(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-0cb51664"],{"21a9":function(t,e,s){"use strict";var a=s("94d4"),i=s.n(a);i.a},2452:function(t,e,s){"use strict";var a=s("62f6"),i=s.n(a);i.a},"3cbc":function(t,e,s){"use strict";var a=function(){var t=this,e=t.$createElement,s=t._self._c||e;return s("div",{staticClass:"pan-item",style:{zIndex:t.zIndex,height:t.height,width:t.width}},[s("div",{staticClass:"pan-info"},[s("div",{staticClass:"pan-info-roles-container"},[t._t("default")],2)]),t._v(" "),s("img",{staticClass:"pan-thumb",attrs:{src:t.image}})])},i=[],r=(s("8f42"),{name:"PanThumb",props:{image:{type:String,required:!0},zIndex:{type:Number,default:1},width:{type:String,default:"150px"},height:{type:String,default:"150px"}}}),c=r,n=(s("2452"),s("6691")),l=Object(n["a"])(c,a,i,!1,null,"0d3d578f",null);e["a"]=l.exports},"62f6":function(t,e,s){},"6f92":function(t,e,s){"use strict";var a=s("82f5"),i=s.n(a);i.a},"82f5":function(t,e,s){},"94d4":function(t,e,s){},ecac:function(t,e,s){"use strict";s.r(e);var a=function(){var t=this,e=t.$createElement,s=t._self._c||e;return s("div",{staticClass:"app-container"},[t.user?s("div",[s("el-row",{attrs:{gutter:20}},[s("el-col",{attrs:{span:6}},[s("user-card",{attrs:{user:t.user}})],1),t._v(" "),s("el-col",{attrs:{span:18}},[s("el-card",[s("el-tabs",{model:{value:t.activeTab,callback:function(e){t.activeTab=e},expression:"activeTab"}},[s("el-tab-pane",{attrs:{label:t.$t("profile.activity"),name:"activity"}},[s("activity")],1),t._v(" "),s("el-tab-pane",{attrs:{label:t.$t("profile.timeline"),name:"timeline"}},[s("timeline")],1),t._v(" "),s("el-tab-pane",{attrs:{label:t.$t("profile.account"),name:"account"}},[s("account",{attrs:{user:t.user}})],1)],1)],1)],1)],1)],1):t._e()])},i=[],r=(s("efce"),s("4634"),s("ed8b"),s("7cfd"),s("97a3")),c=s("52c1"),n=function(){var t=this,e=t.$createElement,s=t._self._c||e;return s("el-card",[s("div",{staticClass:"clearfix",attrs:{slot:"header"},slot:"header"},[s("span",[t._v("关于")])]),t._v(" "),s("div",{staticClass:"user-profile"},[s("div",{staticClass:"box-center"},[s("pan-thumb",{attrs:{height:"100px",hoverable:!1,image:t.user.avatar,width:"100px"}},[s("div",[t._v("您好")]),t._v("\n        "+t._s(t.user.role)+"\n      ")])],1),t._v(" "),s("div",{staticClass:"box-center"},[s("div",{staticClass:"user-name text-center"},[t._v(t._s(t.user.name))]),t._v(" "),s("div",{staticClass:"user-role text-center text-muted"},[t._v(t._s(t._f("uppercaseFirst")(t.user.role)))])])]),t._v(" "),s("div",{staticClass:"user-bio"},[s("div",{staticClass:"user-education user-bio-section"},[s("div",{staticClass:"user-bio-section-header"},[s("svg-icon",{attrs:{"icon-class":"component"}}),t._v(" "),s("span",[t._v("签名")])],1),t._v(" "),s("div",{staticClass:"user-bio-section-body"},[s("div",{staticClass:"text-muted"},[t._v("个人信息页面")])])]),t._v(" "),s("div",{staticClass:"user-skills user-bio-section"},[s("div",{staticClass:"user-bio-section-header"},[s("svg-icon",{attrs:{"icon-class":"skill"}}),t._v(" "),s("span",[t._v("进度")])],1),t._v(" "),s("div",{staticClass:"user-bio-section-body"},[s("div",{staticClass:"progress-item"},[s("span",[t._v("A")]),t._v(" "),s("el-progress",{attrs:{percentage:70}})],1),t._v(" "),s("div",{staticClass:"progress-item"},[s("span",[t._v("B")]),t._v(" "),s("el-progress",{attrs:{percentage:18}})],1),t._v(" "),s("div",{staticClass:"progress-item"},[s("span",[t._v("C")]),t._v(" "),s("el-progress",{attrs:{percentage:12}})],1),t._v(" "),s("div",{staticClass:"progress-item"},[s("span",[t._v("D")]),t._v(" "),s("el-progress",{attrs:{percentage:100,status:"success"}})],1)])])])])},l=[],o=s("3cbc"),u={components:{PanThumb:o["a"]},props:{user:{type:Object,default:function(){return{name:"",email:"",avatar:"",roles:""}}}}},p=u,v=(s("6f92"),s("6691")),m=Object(v["a"])(p,n,l,!1,null,"058a42a6",null),d=m.exports,f=function(){var t=this,e=t.$createElement,s=t._self._c||e;return s("div",{staticClass:"user-activity"},[s("div",{staticClass:"post"},[s("div",{staticClass:"user-block"},[s("img",{staticClass:"img-circle",attrs:{src:"https://wpimg.wallstcn.com/57ed425a-c71e-4201-9428-68760c0537c4.jpg"+t.avatarPrefix}}),t._v(" "),s("span",{staticClass:"username text-muted"},[t._v("用户A")]),t._v(" "),s("span",{staticClass:"description"},[t._v("7:30pm")])]),t._v(" "),s("p",[t._v("此处用于填写文章内容.")]),t._v(" "),s("ul",{staticClass:"list-inline"},[t._m(0),t._v(" "),s("li",[s("span",{staticClass:"link-black text-sm"},[s("svg-icon",{attrs:{"icon-class":"like"}})],1)])])]),t._v(" "),s("div",{staticClass:"post"},[s("div",{staticClass:"user-block"},[s("img",{staticClass:"img-circle",attrs:{src:"https://wpimg.wallstcn.com/9e2a5d0a-bd5b-457f-ac8e-86554616c87b.jpg"+t.avatarPrefix}}),t._v(" "),s("span",{staticClass:"username text-muted"},[t._v("用户B")]),t._v(" "),s("span",{staticClass:"description"},[t._v("昨天")])]),t._v(" "),s("p",[t._v("此处用于填写文章内容")]),t._v(" "),s("ul",{staticClass:"list-inline"},[t._m(1),t._v(" "),s("li",[s("span",{staticClass:"link-black text-sm"},[s("svg-icon",{attrs:{"icon-class":"like"}})],1)])])]),t._v(" "),s("div",{staticClass:"post"},[s("div",{staticClass:"user-block"},[s("img",{staticClass:"img-circle",attrs:{src:"https://wpimg.wallstcn.com/fb57f689-e1ab-443c-af12-8d4066e202e2.jpg"+t.avatarPrefix}}),t._v(" "),s("span",{staticClass:"username"},[t._v("用户C")]),t._v(" "),s("span",{staticClass:"description"},[t._v("2天前")])]),t._v(" "),s("div",{staticClass:"user-images"},[s("el-carousel",{attrs:{interval:6e3,height:"220px",type:"card"}},t._l(t.carouselImages,(function(e){return s("el-carousel-item",{key:e},[s("img",{staticClass:"image",attrs:{src:e+t.carouselPrefix}})])})),1)],1),t._v(" "),s("ul",{staticClass:"list-inline"},[t._m(2),t._v(" "),s("li",[s("span",{staticClass:"link-black text-sm"},[s("svg-icon",{attrs:{"icon-class":"like"}})],1)])])])])},_=[function(){var t=this,e=t.$createElement,s=t._self._c||e;return s("li",[s("span",{staticClass:"link-black text-sm"},[s("i",{staticClass:"el-icon-share"})])])},function(){var t=this,e=t.$createElement,s=t._self._c||e;return s("li",[s("span",{staticClass:"link-black text-sm"},[s("i",{staticClass:"el-icon-share"})])])},function(){var t=this,e=t.$createElement,s=t._self._c||e;return s("li",[s("span",{staticClass:"link-black text-sm"},[s("i",{staticClass:"el-icon-share"})])])}],b="?imageView2/1/w/80/h/80",g="?imageView2/2/h/440",h={data:function(){return{carouselImages:["https://wpimg.wallstcn.com/9679ffb0-9e0b-4451-9916-e21992218054.jpg","https://wpimg.wallstcn.com/bcce3734-0837-4b9f-9261-351ef384f75a.jpg","https://wpimg.wallstcn.com/d1d7b033-d75e-4cd6-ae39-fcd5f1c0a7c5.jpg","https://wpimg.wallstcn.com/50530061-851b-4ca5-9dc5-2fead928a939.jpg"],avatarPrefix:b,carouselPrefix:g}}},C=h,x=(s("21a9"),Object(v["a"])(C,f,_,!1,null,"3d4f3b9c",null)),w=x.exports,y=function(){var t=this,e=t.$createElement,s=t._self._c||e;return s("div",{staticClass:"block"},[s("el-timeline",t._l(t.timeline,(function(e,a){return s("el-timeline-item",{key:a,attrs:{timestamp:e.timestamp,placement:"top"}},[s("el-card",[s("h4",[t._v(t._s(e.title))]),t._v(" "),s("p",[t._v(t._s(e.content))])])],1)})),1)],1)},k=[],j={data:function(){return{timeline:[{timestamp:"2019/5/14",title:"初版完成",content:"serfend committed 2019/4/20 20:46"},{timestamp:"2019/5/7",title:"基本完成初版",content:"无内容"},{timestamp:"2019/4/28",title:"整合需求开始开发",content:"无内容"},{timestamp:"2019/4/23",title:"提出干部休假系统需求，开始制作",content:"无内容"}]}}},O=j,P=Object(v["a"])(O,y,k,!1,null,null,null),$=P.exports,E=function(){var t=this,e=t.$createElement,s=t._self._c||e;return s("el-form",[s("el-form-item",{attrs:{label:"姓名"}},[s("el-input",{model:{value:t.user.name,callback:function(e){t.$set(t.user,"name","string"===typeof e?e.trim():e)},expression:"user.name"}})],1),t._v(" "),s("el-form-item",{attrs:{label:"Email"}},[s("el-input",{model:{value:t.user.email,callback:function(e){t.$set(t.user,"email","string"===typeof e?e.trim():e)},expression:"user.email"}})],1),t._v(" "),s("el-form-item",[s("el-button",{attrs:{type:"primary"},on:{click:t.submit}},[t._v("修改")])],1)],1)},T=[],D={props:{user:{type:Object,default:function(){return{name:"",email:""}}}},methods:{submit:function(){this.$message({message:"功能被禁用，修改数据失败",type:"warning",duration:5e3})}}},I=D,S=Object(v["a"])(I,E,T,!1,null,null,null),A=S.exports;function z(t,e){var s=Object.keys(t);if(Object.getOwnPropertySymbols){var a=Object.getOwnPropertySymbols(t);e&&(a=a.filter((function(e){return Object.getOwnPropertyDescriptor(t,e).enumerable}))),s.push.apply(s,a)}return s}function U(t){for(var e=1;e<arguments.length;e++){var s=null!=arguments[e]?arguments[e]:{};e%2?z(s,!0).forEach((function(e){Object(r["a"])(t,e,s[e])})):Object.getOwnPropertyDescriptors?Object.defineProperties(t,Object.getOwnPropertyDescriptors(s)):z(s).forEach((function(e){Object.defineProperty(t,e,Object.getOwnPropertyDescriptor(s,e))}))}return t}var B={name:"Profile",components:{UserCard:d,Activity:w,Timeline:$,Account:A},data:function(){return{user:{},activeTab:"activity"}},computed:U({},Object(c["b"])(["name","avatar","roles"])),created:function(){this.getUser()},methods:{getUser:function(){this.user={name:this.name,role:this.roles.join(" | "),email:"admin@test.com",avatar:this.avatar}}}},J=B,V=Object(v["a"])(J,a,i,!1,null,null,null);e["default"]=V.exports}}]);