﻿const search = new Vue({
    el: '#search',
    data: {
        currentAppUserName: document.querySelector('#body').dataset.appUser,
        appPermission: document.querySelector('#body').dataset.appPermission,
        preloader: document.querySelector('#preloader'),
        message: {
            element: document.querySelector('#message'),
            text: document.querySelector('#message .message-text')
        },
        current: null,
        commenting: false,

        canShare: navigator.share,

        iso: null,
        exposure: null,
        aperture: null,
        focalLength: null,
        tags: '',

        projectsTab: {
            search: '',
            active: false,
            photos: [],
            loaded: false,
            incallback: false,
            page: 0
        },
        tagsTab: {
            search: '',
            active: false,
            tags: []
        },
        usersTab: {
            search: '',
            active: true,
            users: [],
            loaded: false,
            incallback: false,
            page: 0
        },
        modals: {
            likeActive: false,
            metadataActive: false
        }
    },
    mounted() {
        window.addEventListener('scroll', this.autoSearch);
    },
    created() {
        this.search();
    },
    computed: {
        filteredTags() {
            if (this.tagsTab.search) {
                return this.tagsTab.tags.filter(t => {
                    return t.name.toLowerCase().includes(this.tagsTab.search.toLowerCase())
                });
            }

            return this.tagsTab.tags;
        }
    },
    methods: {
        search() {
            if (this.usersTab.active && !this.usersTab.incallback && this.usersTab.page > -1 && !this.usersTab.loaded) {

                this.usersTab.incallback = true;
                this.preloader.setAttribute('data-hidden', 'false');

                this.$http.get(`/api/users/search?search=${this.usersTab.search}&page=${this.usersTab.page}`).then(response => response.json()).then(json => {

                    if (this.usersTab.users == null)
                        this.usersTab.users = json;
                    else if (json.length == 0)
                        this.usersTab.loaded = true;
                    else {
                        for (let user in json)
                            this.usersTab.users.push(json[user]);
                    }

                    this.preloader.setAttribute('data-hidden', 'true');
                    this.usersTab.incallback = false;
                    this.usersTab.page++;
                },
                error => {
                    this.preloader.setAttribute('data-hidden', 'true');
                    this.usersTab.incallback = false;

                    this.message.text.innerHTML = 'Error while loading users';
                    this.message.element.setAttribute('data-message-type', 'error');
                    this.message.element.setAttribute('data-hidden', 'false');
                    setTimeout(() => { this.message.element.setAttribute('data-hidden', 'true'); }, 5000);
                });
            }
            else if (this.projectsTab.active && !this.projectsTab.incallback && this.projectsTab.page > -1 && !this.projectsTab.loaded) {
                this.projectsTab.incallback = true;
                this.preloader.setAttribute('data-hidden', 'false');

                this.$http.get(`/api/photos/search?search=${this.projectsTab.search}&page=${this.projectsTab.page}&iso=${this.iso}&exposure=${this.exposure}&aperture=${this.aperture}&focalLength=${this.focalLength}`).then(response => response.json()).then(json => {

                    if (this.projectsTab.photos == null)
                        this.projectsTab.photos = json;
                    else if (json.length == 0)
                        this.projectsTab.loaded = true;
                    else {
                        for (let photo in json)
                            this.projectsTab.photos.push(json[photo]);
                    }

                    this.preloader.setAttribute('data-hidden', 'true');
                    this.projectsTab.incallback = false;
                    this.projectsTab.page++;
                },
                    error => {
                        this.preloader.setAttribute('data-hidden', 'true');
                        this.projectsTab.incallback = false;

                        this.message.text.innerHTML = 'Error while loading photos';
                        this.message.element.setAttribute('data-message-type', 'error');
                        this.message.element.setAttribute('data-hidden', 'false');
                        setTimeout(() => { this.message.element.setAttribute('data-hidden', 'true'); }, 5000);
                    });
            }
            else if (this.tagsTab.active) {
                this.preloader.setAttribute('data-hidden', 'false');

                this.$http.get(`/api/tags`).then(response => response.json()).then(json => {
                    this.tagsTab.tags = json;

                    this.preloader.setAttribute('data-hidden', 'true');
                },
                error => {
                    this.preloader.setAttribute('data-hidden', 'true');

                    this.message.text.innerHTML = 'Error while loading tags';
                    this.message.element.setAttribute('data-message-type', 'error');
                    this.message.element.setAttribute('data-hidden', 'false');
                    setTimeout(() => { this.message.element.setAttribute('data-hidden', 'true'); }, 5000);
                });
            }
        },
        like(post) {
            if (!this.currentAppUserName)
                return -1;

            if (!post.liked) {
                post.liked = true;
                post.likes.push({
                    date: this.getCurrentDate(),
                    owner: {
                        userName: this.currentAppUserName
                    }
                });
                this.$http.post(`/api/likes/add/${post.$id}`).then(response => {

                }, response => {
                    this.message.text.innerHTML = 'Error while liking photo';
                    this.message.element.setAttribute('data-message-type', 'error');
                    this.message.element.setAttribute('data-hidden', 'false');
                    setTimeout(() => { this.message.element.setAttribute('data-hidden', 'true'); }, 5000);

                    post.liked = false;
                    for (let i in post.likes) {
                        if (post.likes[i].owner.userName == this.currentAppUserName)
                            post.likes.splice(i, 1);
                    }
                });
            }
        },
        dislike(post) {
            if (!this.currentAppUserName)
                return -1;

            if (post.liked) {
                post.liked = false;
                for (let i in post.likes) {
                    if (post.likes[i].owner.userName == this.currentAppUserName)
                        post.likes.splice(i, 1);
                }
                this.$http.post(`/api/likes/delete/${post.$id}`).then(response => {

                }, response => {
                    this.message.text.innerHTML = 'Error while disliking photo';
                    this.message.element.setAttribute('data-message-type', 'error');
                    this.message.element.setAttribute('data-hidden', 'false');
                    setTimeout(() => { this.message.element.setAttribute('data-hidden', 'true'); }, 5000);

                    post.liked = true;
                    post.likes.push({
                        date: this.getCurrentDate(),
                        owner: {
                            userName: this.currentAppUserName
                        }
                    });
                });
            }
        },
        comment(post) {
            if (!this.currentAppUserName)
                return -1;

            const text = document.querySelector(`#comment${post.$id}`).value;

            if (text == '' || text.length < 1)
                return -1;

            this.commenting = true;
            this.preloader.setAttribute('data-hidden', 'false');

            const words = text.split(/[, ;.]/);;

            let ok = true;

            for (let word in words) {
                if (words[word].length > 12)
                    ok = false;
            }

            if (ok) {
                this.$http.post(`/api/comments/add?photoId=${post.$id}&text=${text}`).then(response => response.json()).then(json => {
                    post.comments.push({
                        $id: json,
                        text: text,
                        date: this.getCurrentDate(),
                        owner: {
                            userName: this.currentAppUserName
                        }
                    });

                    document.querySelector(`#comment${post.$id}`).value = '';
                    this.commenting = false;
                    this.preloader.setAttribute('data-hidden', 'true');
                }, response => {
                    this.message.text.innerHTML = 'Error while commenting photo';
                    this.message.element.setAttribute('data-message-type', 'error');
                    this.message.element.setAttribute('data-hidden', 'false');
                    setTimeout(() => { this.message.element.setAttribute('data-hidden', 'true'); }, 5000);

                    this.commenting = false;
                    this.preloader.setAttribute('data-hidden', 'true');
                });
            }
            else {
                this.preloader.setAttribute('data-hidden', 'true');
                this.commenting = false;
            }
        },
        deleteComment(comment, post) {
            if (this.currentAppUserName == comment.owner.userName || post.owner.userName == this.currentAppUserName || this.appPermission) {
                this.preloader.setAttribute('data-hidden', 'false');
                this.$http.post(`/api/comments/delete/${comment.$id}`).then(response => {
                    for (let i in post.comments) {
                        if (post.comments[i].$id == comment.$id)
                            post.comments.splice(i, 1);
                    }
                    this.preloader.setAttribute('data-hidden', 'true');
                }, response => {
                    this.preloader.setAttribute('data-hidden', 'true');
                    this.message.text.innerHTML = 'Error while deleting comment';
                    this.message.element.setAttribute('data-message-type', 'error');
                    this.message.element.setAttribute('data-hidden', 'false');
                    setTimeout(() => { this.message.element.setAttribute('data-hidden', 'true'); }, 5000);
                });
            }
        },
        copyToClipboard(id) {
            const copyTextArea = document.createElement('textarea');
            copyTextArea.value = `https://photoub.azurewebsites.net/photos/${id}`;
            document.body.appendChild(copyTextArea);
            copyTextArea.select();

            const successful = document.execCommand('copy');

            document.body.removeChild(copyTextArea);

            const icon = document.querySelector(`i[data-copy-id="${id}"]`);

            icon.classList.remove('far');
            icon.classList.remove('fa-clipboard');
            icon.classList.remove('fas');
            icon.classList.remove('fa-clipboard-check');

            icon.classList.add('fas');
            icon.classList.add('fa-clipboard-check');
        },
        reportPhoto(id) {
            let text = '';

            const icon = document.querySelector(`i[data-report-id="${id}"]`);

            this.$http.post(`/api/photos/report/${id}?text=${text}`).then(response => {
                icon.classList.remove('fas');
                icon.classList.remove('fa-exclamation-circle');
                icon.classList.remove('fa-check-circle');

                icon.classList.add('fas');
                icon.classList.add('fa-check-circle');
            }, response => {

            });
        },
        autoSearch() {
            const scrollTop = document.documentElement.scrollTop;
            const windowHeight = window.innerHeight;
            let bodyHeight = document.documentElement.scrollHeight - windowHeight;
            let scrollPercentage = (scrollTop / bodyHeight);

            // If the scroll is more than 60% from the top, load more content. Load users
            if (this.usersTab.active && !this.usersTab.loaded && this.usersTab.users.length % 12 == 0 && scrollPercentage > 0.6)
                this.search();
            else if (this.projectsTab.active && !this.projectsTab.loaded && this.projectsTab.photos.length % 8 == 0 && scrollPercentage > 0.6)
                this.search();
        },
        handleSearch(str) {
            if (this.usersTab.active && (this.usersTab.search.length > 2 || !this.usersTab.search.length)) {
                this.usersTab.users = null;
                this.usersTab.page = 0;
                this.usersTab.loaded = false;
                this.search();
            }
            else if (this.projectsTab.active && (this.projectsTab.search.length > 2 || !this.projectsTab.search.length)) {
                this.projectsTab.photos = null;
                this.projectsTab.page = 0;
                this.projectsTab.loaded = false;
                this.search();
            }
        },

        follow(user) {
            if (!this.currentAppUserName)
                return -1;

            if (!user.followed) {
                user.followed = true;
                this.$http.post(`/api/users/follow/${user.userName}`).then(response => {

                }, response => {

                    this.message.text.innerHTML = 'Error while following';
                    this.message.element.setAttribute('data-message-type', 'error');
                    this.message.element.setAttribute('data-hidden', 'false');
                    setTimeout(() => { this.message.element.setAttribute('data-hidden', 'true'); }, 5000);

                    user.followed = false;
                });
            }
        },
        disfollow(user) {
            if (!this.currentAppUserName)
                return -1;

            if (user.followed) {
                user.followed = false;

                this.$http.post(`/api/users/dismiss/follow/${user.userName}`).then(response => {

                }, response => {

                    this.message.text.innerHTML = 'Error while unfollowing';
                    this.message.element.setAttribute('data-message-type', 'error');
                    this.message.element.setAttribute('data-hidden', 'false');
                    setTimeout(() => { this.message.element.setAttribute('data-hidden', 'true'); }, 5000);

                    user.followed = true;
                });
            }
        },

        bookmark(post) {
            if (!this.currentAppUserName)
                return -1;

            if (!post.bookmarked) {
                post.bookmarked = true;

                this.$http.post(`/api/photos/bookmark/${post.$id}`).then(response => {

                }, response => {

                    this.message.text.innerHTML = 'Error while bookmarking';
                    this.message.element.setAttribute('data-message-type', 'error');
                    this.message.element.setAttribute('data-hidden', 'false');
                    setTimeout(() => { this.message.element.setAttribute('data-hidden', 'true'); }, 5000);

                    post.bookmarked = false;
                });
            }
        },
        disbookmark(post) {
            if (!this.currentAppUserName)
                return -1;

            if (post.bookmarked) {
                post.bookmarked = false;

                this.$http.post(`/api/photos/dismiss/bookmark/${post.$id}`).then(response => {

                }, response => {
                    this.message.text.innerHTML = 'Error while deleting bookmark';
                    this.message.element.setAttribute('data-message-type', 'error');
                    this.message.element.setAttribute('data-hidden', 'false');
                    setTimeout(() => { this.message.element.setAttribute('data-hidden', 'true'); }, 5000);

                    post.bookmarked = true;
                });
            }
        },

        sharePhoto(photo) {
            if (this.canShare) {
                navigator.share({
                    title: `${photo.owner.userName}'s photo`,
                    text: photo.description,
                    url: `https://TrainSchdule.azurewebsites.net/photos/${photo.$id}`,
                })
                    .then(() => console.log('Successful share'))
                    .catch((error) => console.log('Error sharing', error));
            }
        },

        showLikes(post) {
            this.modals.likeActive = true;
            this.current = post;
            document.documentElement.classList.add('is-clipped');
            document.body.classList.add('is-clipped');
        },
        showComments(post) {
            post.commentActive = post.commentActive ? false : true;
        },
        showMetadata(post) {
            this.modals.metadataActive = true;
            this.current = post;
            document.documentElement.classList.add('is-clipped');
            document.body.classList.add('is-clipped');
        },

        showUsers() {
            this.usersTab.active = true;
            this.projectsTab.active = false;
            this.tagsTab.active = false;

            if(!this.usersTab.users)
                this.handleSearch();
        },
        showPhotos() {
            this.usersTab.active = false;
            this.tagsTab.active = false;
            this.projectsTab.active = true;

            if (!this.projectsTab.photos || this.projectsTab.photos.length <= 0)
                this.handleSearch();
        },
        showTags() {
            this.usersTab.active = false;
            this.projectsTab.active = false;
            this.tagsTab.active = true;

            if (!this.tagsTab.tags || this.tagsTab.tags.length <= 0)
                this.search();
        },

        closeLikes() {
            this.modals.likeActive = false;
            document.documentElement.classList.remove('is-clipped');
            document.body.classList.remove('is-clipped');
        },
        closeMetadata() {
            this.modals.metadataActive = false;
            document.documentElement.classList.remove('is-clipped');
            document.body.classList.remove('is-clipped');
        },

        getCurrentDate() {
            const date = new Date();

            const monthNames = [
                "January", "February", "March",
                "April", "May", "June", "July",
                "August", "September", "October",
                "November", "December"
            ];

            const day = date.getDate();
            const monthIndex = date.getMonth();
            const year = date.getFullYear();

            return `${monthNames[monthIndex]} ${day}, ${year}`;
        }
    }
});