(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-aea3e948"],{1131:function(t,e,n){"use strict";var s=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("el-dropdown",{staticClass:"international",attrs:{trigger:"click"},on:{command:t.handleSetLanguage}},[n("div",[n("svg-icon",{attrs:{"class-name":"international-icon","icon-class":"language"}})],1),t._v(" "),n("el-dropdown-menu",{attrs:{slot:"dropdown"},slot:"dropdown"},[n("el-dropdown-item",{attrs:{disabled:"zh"===t.language,command:"zh"}},[t._v("\n      中文\n    ")]),t._v(" "),n("el-dropdown-item",{attrs:{disabled:"en"===t.language,command:"en"}},[t._v("\n      English\n    ")]),t._v(" "),n("el-dropdown-item",{attrs:{disabled:"es"===t.language,command:"es"}},[t._v("\n      Español\n    ")])],1)],1)},a=[],o={computed:{language:function(){return this.$store.getters.language}},methods:{handleSetLanguage:function(t){this.$i18n.locale=t,this.$store.dispatch("app/setLanguage",t),this.$message({message:"Switch Language Success",type:"success"})}}},i=o,r=n("17cc"),c=Object(r["a"])(i,s,a,!1,null,null,null);e["a"]=c.exports},2017:function(t,e,n){"use strict";var s=n("a428"),a=n.n(s);a.a},5961:function(t,e,n){},"9ed6":function(t,e,n){"use strict";n.r(e);var s=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",{staticClass:"login-container"},[n("el-form",{ref:"loginForm",staticClass:"login-form",attrs:{model:t.loginForm,rules:t.loginRules,"auto-complete":"on","label-position":"left"}},[n("div",{staticClass:"title-container"},[n("h3",{staticClass:"title"},[t._v(t._s(t.$t("login.title")))]),t._v(" "),n("lang-select",{staticClass:"set-language"})],1),t._v(" "),n("el-form-item",{attrs:{prop:"username"}},[n("span",{staticClass:"svg-container"},[n("svg-icon",{attrs:{"icon-class":"user"}})],1),t._v(" "),n("el-input",{ref:"username",attrs:{placeholder:t.$t("login.username"),"auto-complete":"on",name:"username",tabindex:"1",type:"text"},model:{value:t.loginForm.username,callback:function(e){t.$set(t.loginForm,"username",e)},expression:"loginForm.username"}})],1),t._v(" "),n("el-tooltip",{attrs:{content:"Caps lock is On",manual:"",placement:"right"},model:{value:t.capsTooltip,callback:function(e){t.capsTooltip=e},expression:"capsTooltip"}},[n("el-form-item",{attrs:{prop:"password"}},[n("span",{staticClass:"svg-container"},[n("svg-icon",{attrs:{"icon-class":"password"}})],1),t._v(" "),n("el-input",{key:t.passwordType,ref:"password",attrs:{placeholder:t.$t("login.password"),type:t.passwordType,"auto-complete":"on",name:"password",tabindex:"2"},on:{blur:function(e){t.capsTooltip=!1}},nativeOn:{keyup:[function(e){return!e.type.indexOf("key")&&t._k(e.keyCode,"enter",13,e.key,"Enter")?null:t.handleLogin(e)},function(e){return t.checkCapslock(e)}]},model:{value:t.loginForm.password,callback:function(e){t.$set(t.loginForm,"password",e)},expression:"loginForm.password"}}),t._v(" "),n("span",{staticClass:"show-pwd",on:{click:t.showPwd}},[n("svg-icon",{attrs:{"icon-class":"password"===t.passwordType?"eye":"eye-open"}})],1)],1)],1),t._v(" "),n("el-button",{staticStyle:{width:"100%","margin-bottom":"30px"},attrs:{loading:t.loading,type:"primary"},nativeOn:{click:function(e){return e.preventDefault(),t.handleLogin(e)}}},[t._v(t._s(t.$t("login.logIn")))]),t._v(" "),n("el-button",{staticStyle:{width:"100%","margin-bottom":"5px","margin-left":"0px"},attrs:{disabled:!0,type:"primary"},nativeOn:{click:function(e){return e.preventDefault(),t.handleReg(e)}}},[t._v(t._s(t.$t("register.title")))])],1),t._v(" "),n("el-dialog",{attrs:{title:t.$t("login.thirdparty"),visible:t.showDialog},on:{"update:visible":function(e){t.showDialog=e}}},[t._v("\n    "+t._s(t.$t("login.thirdpartyTips"))+"\n    "),n("br"),t._v(" "),n("br"),t._v(" "),n("br"),t._v(" "),n("social-sign")],1)],1)},a=[],o=n("61f7"),i=n("1131"),r=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",{staticClass:"social-signup-container"},[n("div",{staticClass:"sign-btn",on:{click:function(e){return t.wechatHandleClick("wechat")}}},[n("span",{staticClass:"wx-svg-container"},[n("svg-icon",{staticClass:"icon",attrs:{"icon-class":"wechat"}})],1),t._v("\n    WeChat\n  ")]),t._v(" "),n("div",{staticClass:"sign-btn",on:{click:function(e){return t.tencentHandleClick("tencent")}}},[n("span",{staticClass:"qq-svg-container"},[n("svg-icon",{staticClass:"icon",attrs:{"icon-class":"qq"}})],1),t._v("\n    QQ\n  ")])])},c=[],l={name:"SocialSignin",methods:{wechatHandleClick:function(t){alert("ok")},tencentHandleClick:function(t){alert("ok")}}},u=l,d=(n("edc1"),n("17cc")),p=Object(d["a"])(u,r,c,!1,null,"c817cede",null),g=p.exports,m=n("7105"),f=n("c20c"),h={name:"Login",components:{LangSelect:i["a"],SocialSign:g},data:function(){var t=function(t,e,n){Object(o["d"])(e)?n():n(new Error("Please enter the correct user name"))},e=function(t,e,n){e.length<6?n(new Error("The password can not be less than 6 digits")):n()};return{loginForm:{username:"1000000",password:"11111111",RememberMe:!1,verify:201700816},loginRules:{username:[{required:!0,trigger:"blur",validator:t}],password:[{required:!0,trigger:"blur",validator:e}]},passwordType:"password",capsTooltip:!1,loading:!1,showDialog:!1,redirect:void 0}},watch:{$route:{handler:function(t){this.redirect=t.query&&t.query.redirect},immediate:!0}},created:function(){},mounted:function(){var t=this;""===this.loginForm.username?this.$refs.username.focus():""===this.loginForm.password&&this.$refs.password.focus(),Object(f["a"])().then(function(e){e&&t.$router.push(t.redirect)}).catch(function(){Object(m["Message"])({message:"注意，请务必不要泄露密码",type:"warning",duration:5e3})})},destroyed:function(){},methods:{checkCapslock:function(){var t=arguments.length>0&&void 0!==arguments[0]?arguments[0]:{},e=t.shiftKey,n=t.key;n&&1===n.length&&(this.capsTooltip=!!(e&&n>="a"&&n<="z"||!e&&n>="A"&&n<="Z")),"CapsLock"===n&&!0===this.capsTooltip&&(this.capsTooltip=!1)},showPwd:function(){var t=this;"password"===this.passwordType?this.passwordType="":this.passwordType="password",this.$nextTick(function(){t.$refs.password.focus()})},handleReg:function(){this.$router.push({path:"/register"})},handleLogin:function(){var t=this;this.$refs.loginForm.validate(function(e){if(!e)return Object(m["Message"])({message:"无效的登录请求",type:"error",duration:5e3}),!1;t.loading=!0,t.$store.dispatch("user/login",t.loginForm).then(function(e){t.loading=!1,Object(m["Message"])({message:t.$t("login.success"),type:"success",duration:5e3}),t.$router.push({path:t.redirect||"/"})}).catch(function(){Object(m["Message"])({message:"无效的登录请求",type:"error",duration:5e3}),t.loading=!1})})}}},v=h,w=(n("2017"),n("e9d1"),Object(d["a"])(v,s,a,!1,null,"1a23f5ab",null));e["default"]=w.exports},a428:function(t,e,n){},c20c:function(t,e,n){"use strict";n.d(e,"a",function(){return a}),n.d(e,"e",function(){return o}),n.d(e,"c",function(){return i}),n.d(e,"b",function(){return r}),n.d(e,"d",function(){return c});var s=n("b775");function a(t){return s["a"].get("users/base",{params:{id:t}})}function o(t){return s["a"].get("users/social",{params:{id:t}})}function i(t){return s["a"].get("users/duties",{params:{id:t}})}function r(t){return s["a"].get("users/company",{params:{id:t}})}function c(t){return s["a"].get("/account/GetUserIdByCid",{params:{cid:t}})}},e9d1:function(t,e,n){"use strict";var s=n("f168"),a=n.n(s);a.a},edc1:function(t,e,n){"use strict";var s=n("5961"),a=n.n(s);a.a},f168:function(t,e,n){}}]);