 // ========== Owl Carousel v2.3.4 
!function(a,b,c,d){function e(b,c){this.settings=null,this.options=a.extend({},e.Defaults,c),this.$element=a(b),this._handlers={},this._plugins={},this._supress={},this._current=null,this._speed=null,this._coordinates=[],this._breakpoint=null,this._width=null,this._items=[],this._clones=[],this._mergers=[],this._widths=[],this._invalidated={},this._pipe=[],this._drag={time:null,target:null,pointer:null,stage:{start:null,current:null},direction:null},this._states={current:{},tags:{initializing:["busy"],animating:["busy"],dragging:["interacting"]}},a.each(["onResize","onThrottledResize"],a.proxy(function(b,c){this._handlers[c]=a.proxy(this[c],this)},this)),a.each(e.Plugins,a.proxy(function(a,b){this._plugins[a.charAt(0).toLowerCase()+a.slice(1)]=new b(this)},this)),a.each(e.Workers,a.proxy(function(b,c){this._pipe.push({filter:c.filter,run:a.proxy(c.run,this)})},this)),this.setup(),this.initialize()}e.Defaults={items:3,loop:!1,center:!1,rewind:!1,checkVisibility:!0,mouseDrag:!0,touchDrag:!0,pullDrag:!0,freeDrag:!1,margin:0,stagePadding:0,merge:!1,mergeFit:!0,autoWidth:!1,startPosition:0,rtl:!1,smartSpeed:250,fluidSpeed:!1,dragEndSpeed:!1,responsive:{},responsiveRefreshRate:200,responsiveBaseElement:b,fallbackEasing:"swing",slideTransition:"",info:!1,nestedItemSelector:!1,itemElement:"div",stageElement:"div",refreshClass:"owl-refresh",loadedClass:"owl-loaded",loadingClass:"owl-loading",rtlClass:"owl-rtl",responsiveClass:"owl-responsive",dragClass:"owl-drag",itemClass:"owl-item",stageClass:"owl-stage",stageOuterClass:"owl-stage-outer",grabClass:"owl-grab"},e.Width={Default:"default",Inner:"inner",Outer:"outer"},e.Type={Event:"event",State:"state"},e.Plugins={},e.Workers=[{filter:["width","settings"],run:function(){this._width=this.$element.width()}},{filter:["width","items","settings"],run:function(a){a.current=this._items&&this._items[this.relative(this._current)]}},{filter:["items","settings"],run:function(){this.$stage.children(".cloned").remove()}},{filter:["width","items","settings"],run:function(a){var b=this.settings.margin||"",c=!this.settings.autoWidth,d=this.settings.rtl,e={width:"auto","margin-left":d?b:"","margin-right":d?"":b};!c&&this.$stage.children().css(e),a.css=e}},{filter:["width","items","settings"],run:function(a){var b=(this.width()/this.settings.items).toFixed(3)-this.settings.margin,c=null,d=this._items.length,e=!this.settings.autoWidth,f=[];for(a.items={merge:!1,width:b};d--;)c=this._mergers[d],c=this.settings.mergeFit&&Math.min(c,this.settings.items)||c,a.items.merge=c>1||a.items.merge,f[d]=e?b*c:this._items[d].width();this._widths=f}},{filter:["items","settings"],run:function(){var b=[],c=this._items,d=this.settings,e=Math.max(2*d.items,4),f=2*Math.ceil(c.length/2),g=d.loop&&c.length?d.rewind?e:Math.max(e,f):0,h="",i="";for(g/=2;g>0;)b.push(this.normalize(b.length/2,!0)),h+=c[b[b.length-1]][0].outerHTML,b.push(this.normalize(c.length-1-(b.length-1)/2,!0)),i=c[b[b.length-1]][0].outerHTML+i,g-=1;this._clones=b,a(h).addClass("cloned").appendTo(this.$stage),a(i).addClass("cloned").prependTo(this.$stage)}},{filter:["width","items","settings"],run:function(){for(var a=this.settings.rtl?1:-1,b=this._clones.length+this._items.length,c=-1,d=0,e=0,f=[];++c<b;)d=f[c-1]||0,e=this._widths[this.relative(c)]+this.settings.margin,f.push(d+e*a);this._coordinates=f}},{filter:["width","items","settings"],run:function(){var a=this.settings.stagePadding,b=this._coordinates,c={width:Math.ceil(Math.abs(b[b.length-1]))+2*a,"padding-left":a||"","padding-right":a||""};this.$stage.css(c)}},{filter:["width","items","settings"],run:function(a){var b=this._coordinates.length,c=!this.settings.autoWidth,d=this.$stage.children();if(c&&a.items.merge)for(;b--;)a.css.width=this._widths[this.relative(b)],d.eq(b).css(a.css);else c&&(a.css.width=a.items.width,d.css(a.css))}},{filter:["items"],run:function(){this._coordinates.length<1&&this.$stage.removeAttr("style")}},{filter:["width","items","settings"],run:function(a){a.current=a.current?this.$stage.children().index(a.current):0,a.current=Math.max(this.minimum(),Math.min(this.maximum(),a.current)),this.reset(a.current)}},{filter:["position"],run:function(){this.animate(this.coordinates(this._current))}},{filter:["width","position","items","settings"],run:function(){var a,b,c,d,e=this.settings.rtl?1:-1,f=2*this.settings.stagePadding,g=this.coordinates(this.current())+f,h=g+this.width()*e,i=[];for(c=0,d=this._coordinates.length;c<d;c++)a=this._coordinates[c-1]||0,b=Math.abs(this._coordinates[c])+f*e,(this.op(a,"<=",g)&&this.op(a,">",h)||this.op(b,"<",g)&&this.op(b,">",h))&&i.push(c);this.$stage.children(".active").removeClass("active"),this.$stage.children(":eq("+i.join("), :eq(")+")").addClass("active"),this.$stage.children(".center").removeClass("center"),this.settings.center&&this.$stage.children().eq(this.current()).addClass("center")}}],e.prototype.initializeStage=function(){this.$stage=this.$element.find("."+this.settings.stageClass),this.$stage.length||(this.$element.addClass(this.options.loadingClass),this.$stage=a("<"+this.settings.stageElement+">",{class:this.settings.stageClass}).wrap(a("<div/>",{class:this.settings.stageOuterClass})),this.$element.append(this.$stage.parent()))},e.prototype.initializeItems=function(){var b=this.$element.find(".owl-item");if(b.length)return this._items=b.get().map(function(b){return a(b)}),this._mergers=this._items.map(function(){return 1}),void this.refresh();this.replace(this.$element.children().not(this.$stage.parent())),this.isVisible()?this.refresh():this.invalidate("width"),this.$element.removeClass(this.options.loadingClass).addClass(this.options.loadedClass)},e.prototype.initialize=function(){if(this.enter("initializing"),this.trigger("initialize"),this.$element.toggleClass(this.settings.rtlClass,this.settings.rtl),this.settings.autoWidth&&!this.is("pre-loading")){var a,b,c;a=this.$element.find("img"),b=this.settings.nestedItemSelector?"."+this.settings.nestedItemSelector:d,c=this.$element.children(b).width(),a.length&&c<=0&&this.preloadAutoWidthImages(a)}this.initializeStage(),this.initializeItems(),this.registerEventHandlers(),this.leave("initializing"),this.trigger("initialized")},e.prototype.isVisible=function(){return!this.settings.checkVisibility||this.$element.is(":visible")},e.prototype.setup=function(){var b=this.viewport(),c=this.options.responsive,d=-1,e=null;c?(a.each(c,function(a){a<=b&&a>d&&(d=Number(a))}),e=a.extend({},this.options,c[d]),"function"==typeof e.stagePadding&&(e.stagePadding=e.stagePadding()),delete e.responsive,e.responsiveClass&&this.$element.attr("class",this.$element.attr("class").replace(new RegExp("("+this.options.responsiveClass+"-)\\S+\\s","g"),"$1"+d))):e=a.extend({},this.options),this.trigger("change",{property:{name:"settings",value:e}}),this._breakpoint=d,this.settings=e,this.invalidate("settings"),this.trigger("changed",{property:{name:"settings",value:this.settings}})},e.prototype.optionsLogic=function(){this.settings.autoWidth&&(this.settings.stagePadding=!1,this.settings.merge=!1)},e.prototype.prepare=function(b){var c=this.trigger("prepare",{content:b});return c.data||(c.data=a("<"+this.settings.itemElement+"/>").addClass(this.options.itemClass).append(b)),this.trigger("prepared",{content:c.data}),c.data},e.prototype.update=function(){for(var b=0,c=this._pipe.length,d=a.proxy(function(a){return this[a]},this._invalidated),e={};b<c;)(this._invalidated.all||a.grep(this._pipe[b].filter,d).length>0)&&this._pipe[b].run(e),b++;this._invalidated={},!this.is("valid")&&this.enter("valid")},e.prototype.width=function(a){switch(a=a||e.Width.Default){case e.Width.Inner:case e.Width.Outer:return this._width;default:return this._width-2*this.settings.stagePadding+this.settings.margin}},e.prototype.refresh=function(){this.enter("refreshing"),this.trigger("refresh"),this.setup(),this.optionsLogic(),this.$element.addClass(this.options.refreshClass),this.update(),this.$element.removeClass(this.options.refreshClass),this.leave("refreshing"),this.trigger("refreshed")},e.prototype.onThrottledResize=function(){b.clearTimeout(this.resizeTimer),this.resizeTimer=b.setTimeout(this._handlers.onResize,this.settings.responsiveRefreshRate)},e.prototype.onResize=function(){return!!this._items.length&&(this._width!==this.$element.width()&&(!!this.isVisible()&&(this.enter("resizing"),this.trigger("resize").isDefaultPrevented()?(this.leave("resizing"),!1):(this.invalidate("width"),this.refresh(),this.leave("resizing"),void this.trigger("resized")))))},e.prototype.registerEventHandlers=function(){a.support.transition&&this.$stage.on(a.support.transition.end+".owl.core",a.proxy(this.onTransitionEnd,this)),!1!==this.settings.responsive&&this.on(b,"resize",this._handlers.onThrottledResize),this.settings.mouseDrag&&(this.$element.addClass(this.options.dragClass),this.$stage.on("mousedown.owl.core",a.proxy(this.onDragStart,this)),this.$stage.on("dragstart.owl.core selectstart.owl.core",function(){return!1})),this.settings.touchDrag&&(this.$stage.on("touchstart.owl.core",a.proxy(this.onDragStart,this)),this.$stage.on("touchcancel.owl.core",a.proxy(this.onDragEnd,this)))},e.prototype.onDragStart=function(b){var d=null;3!==b.which&&(a.support.transform?(d=this.$stage.css("transform").replace(/.*\(|\)| /g,"").split(","),d={x:d[16===d.length?12:4],y:d[16===d.length?13:5]}):(d=this.$stage.position(),d={x:this.settings.rtl?d.left+this.$stage.width()-this.width()+this.settings.margin:d.left,y:d.top}),this.is("animating")&&(a.support.transform?this.animate(d.x):this.$stage.stop(),this.invalidate("position")),this.$element.toggleClass(this.options.grabClass,"mousedown"===b.type),this.speed(0),this._drag.time=(new Date).getTime(),this._drag.target=a(b.target),this._drag.stage.start=d,this._drag.stage.current=d,this._drag.pointer=this.pointer(b),a(c).on("mouseup.owl.core touchend.owl.core",a.proxy(this.onDragEnd,this)),a(c).one("mousemove.owl.core touchmove.owl.core",a.proxy(function(b){var d=this.difference(this._drag.pointer,this.pointer(b));a(c).on("mousemove.owl.core touchmove.owl.core",a.proxy(this.onDragMove,this)),Math.abs(d.x)<Math.abs(d.y)&&this.is("valid")||(b.preventDefault(),this.enter("dragging"),this.trigger("drag"))},this)))},e.prototype.onDragMove=function(a){var b=null,c=null,d=null,e=this.difference(this._drag.pointer,this.pointer(a)),f=this.difference(this._drag.stage.start,e);this.is("dragging")&&(a.preventDefault(),this.settings.loop?(b=this.coordinates(this.minimum()),c=this.coordinates(this.maximum()+1)-b,f.x=((f.x-b)%c+c)%c+b):(b=this.settings.rtl?this.coordinates(this.maximum()):this.coordinates(this.minimum()),c=this.settings.rtl?this.coordinates(this.minimum()):this.coordinates(this.maximum()),d=this.settings.pullDrag?-1*e.x/5:0,f.x=Math.max(Math.min(f.x,b+d),c+d)),this._drag.stage.current=f,this.animate(f.x))},e.prototype.onDragEnd=function(b){var d=this.difference(this._drag.pointer,this.pointer(b)),e=this._drag.stage.current,f=d.x>0^this.settings.rtl?"left":"right";a(c).off(".owl.core"),this.$element.removeClass(this.options.grabClass),(0!==d.x&&this.is("dragging")||!this.is("valid"))&&(this.speed(this.settings.dragEndSpeed||this.settings.smartSpeed),this.current(this.closest(e.x,0!==d.x?f:this._drag.direction)),this.invalidate("position"),this.update(),this._drag.direction=f,(Math.abs(d.x)>3||(new Date).getTime()-this._drag.time>300)&&this._drag.target.one("click.owl.core",function(){return!1})),this.is("dragging")&&(this.leave("dragging"),this.trigger("dragged"))},e.prototype.closest=function(b,c){var e=-1,f=30,g=this.width(),h=this.coordinates();return this.settings.freeDrag||a.each(h,a.proxy(function(a,i){return"left"===c&&b>i-f&&b<i+f?e=a:"right"===c&&b>i-g-f&&b<i-g+f?e=a+1:this.op(b,"<",i)&&this.op(b,">",h[a+1]!==d?h[a+1]:i-g)&&(e="left"===c?a+1:a),-1===e},this)),this.settings.loop||(this.op(b,">",h[this.minimum()])?e=b=this.minimum():this.op(b,"<",h[this.maximum()])&&(e=b=this.maximum())),e},e.prototype.animate=function(b){var c=this.speed()>0;this.is("animating")&&this.onTransitionEnd(),c&&(this.enter("animating"),this.trigger("translate")),a.support.transform3d&&a.support.transition?this.$stage.css({transform:"translate3d("+b+"px,0px,0px)",transition:this.speed()/1e3+"s"+(this.settings.slideTransition?" "+this.settings.slideTransition:"")}):c?this.$stage.animate({left:b+"px"},this.speed(),this.settings.fallbackEasing,a.proxy(this.onTransitionEnd,this)):this.$stage.css({left:b+"px"})},e.prototype.is=function(a){return this._states.current[a]&&this._states.current[a]>0},e.prototype.current=function(a){if(a===d)return this._current;if(0===this._items.length)return d;if(a=this.normalize(a),this._current!==a){var b=this.trigger("change",{property:{name:"position",value:a}});b.data!==d&&(a=this.normalize(b.data)),this._current=a,this.invalidate("position"),this.trigger("changed",{property:{name:"position",value:this._current}})}return this._current},e.prototype.invalidate=function(b){return"string"===a.type(b)&&(this._invalidated[b]=!0,this.is("valid")&&this.leave("valid")),a.map(this._invalidated,function(a,b){return b})},e.prototype.reset=function(a){(a=this.normalize(a))!==d&&(this._speed=0,this._current=a,this.suppress(["translate","translated"]),this.animate(this.coordinates(a)),this.release(["translate","translated"]))},e.prototype.normalize=function(a,b){var c=this._items.length,e=b?0:this._clones.length;return!this.isNumeric(a)||c<1?a=d:(a<0||a>=c+e)&&(a=((a-e/2)%c+c)%c+e/2),a},e.prototype.relative=function(a){return a-=this._clones.length/2,this.normalize(a,!0)},e.prototype.maximum=function(a){var b,c,d,e=this.settings,f=this._coordinates.length;if(e.loop)f=this._clones.length/2+this._items.length-1;else if(e.autoWidth||e.merge){if(b=this._items.length)for(c=this._items[--b].width(),d=this.$element.width();b--&&!((c+=this._items[b].width()+this.settings.margin)>d););f=b+1}else f=e.center?this._items.length-1:this._items.length-e.items;return a&&(f-=this._clones.length/2),Math.max(f,0)},e.prototype.minimum=function(a){return a?0:this._clones.length/2},e.prototype.items=function(a){return a===d?this._items.slice():(a=this.normalize(a,!0),this._items[a])},e.prototype.mergers=function(a){return a===d?this._mergers.slice():(a=this.normalize(a,!0),this._mergers[a])},e.prototype.clones=function(b){var c=this._clones.length/2,e=c+this._items.length,f=function(a){return a%2==0?e+a/2:c-(a+1)/2};return b===d?a.map(this._clones,function(a,b){return f(b)}):a.map(this._clones,function(a,c){return a===b?f(c):null})},e.prototype.speed=function(a){return a!==d&&(this._speed=a),this._speed},e.prototype.coordinates=function(b){var c,e=1,f=b-1;return b===d?a.map(this._coordinates,a.proxy(function(a,b){return this.coordinates(b)},this)):(this.settings.center?(this.settings.rtl&&(e=-1,f=b+1),c=this._coordinates[b],c+=(this.width()-c+(this._coordinates[f]||0))/2*e):c=this._coordinates[f]||0,c=Math.ceil(c))},e.prototype.duration=function(a,b,c){return 0===c?0:Math.min(Math.max(Math.abs(b-a),1),6)*Math.abs(c||this.settings.smartSpeed)},e.prototype.to=function(a,b){var c=this.current(),d=null,e=a-this.relative(c),f=(e>0)-(e<0),g=this._items.length,h=this.minimum(),i=this.maximum();this.settings.loop?(!this.settings.rewind&&Math.abs(e)>g/2&&(e+=-1*f*g),a=c+e,(d=((a-h)%g+g)%g+h)!==a&&d-e<=i&&d-e>0&&(c=d-e,a=d,this.reset(c))):this.settings.rewind?(i+=1,a=(a%i+i)%i):a=Math.max(h,Math.min(i,a)),this.speed(this.duration(c,a,b)),this.current(a),this.isVisible()&&this.update()},e.prototype.next=function(a){a=a||!1,this.to(this.relative(this.current())+1,a)},e.prototype.prev=function(a){a=a||!1,this.to(this.relative(this.current())-1,a)},e.prototype.onTransitionEnd=function(a){if(a!==d&&(a.stopPropagation(),(a.target||a.srcElement||a.originalTarget)!==this.$stage.get(0)))return!1;this.leave("animating"),this.trigger("translated")},e.prototype.viewport=function(){var d;return this.options.responsiveBaseElement!==b?d=a(this.options.responsiveBaseElement).width():b.innerWidth?d=b.innerWidth:c.documentElement&&c.documentElement.clientWidth?d=c.documentElement.clientWidth:console.warn("Can not detect viewport width."),d},e.prototype.replace=function(b){this.$stage.empty(),this._items=[],b&&(b=b instanceof jQuery?b:a(b)),this.settings.nestedItemSelector&&(b=b.find("."+this.settings.nestedItemSelector)),b.filter(function(){return 1===this.nodeType}).each(a.proxy(function(a,b){b=this.prepare(b),this.$stage.append(b),this._items.push(b),this._mergers.push(1*b.find("[data-merge]").addBack("[data-merge]").attr("data-merge")||1)},this)),this.reset(this.isNumeric(this.settings.startPosition)?this.settings.startPosition:0),this.invalidate("items")},e.prototype.add=function(b,c){var e=this.relative(this._current);c=c===d?this._items.length:this.normalize(c,!0),b=b instanceof jQuery?b:a(b),this.trigger("add",{content:b,position:c}),b=this.prepare(b),0===this._items.length||c===this._items.length?(0===this._items.length&&this.$stage.append(b),0!==this._items.length&&this._items[c-1].after(b),this._items.push(b),this._mergers.push(1*b.find("[data-merge]").addBack("[data-merge]").attr("data-merge")||1)):(this._items[c].before(b),this._items.splice(c,0,b),this._mergers.splice(c,0,1*b.find("[data-merge]").addBack("[data-merge]").attr("data-merge")||1)),this._items[e]&&this.reset(this._items[e].index()),this.invalidate("items"),this.trigger("added",{content:b,position:c})},e.prototype.remove=function(a){(a=this.normalize(a,!0))!==d&&(this.trigger("remove",{content:this._items[a],position:a}),this._items[a].remove(),this._items.splice(a,1),this._mergers.splice(a,1),this.invalidate("items"),this.trigger("removed",{content:null,position:a}))},e.prototype.preloadAutoWidthImages=function(b){b.each(a.proxy(function(b,c){this.enter("pre-loading"),c=a(c),a(new Image).one("load",a.proxy(function(a){c.attr("src",a.target.src),c.css("opacity",1),this.leave("pre-loading"),!this.is("pre-loading")&&!this.is("initializing")&&this.refresh()},this)).attr("src",c.attr("src")||c.attr("data-src")||c.attr("data-src-retina"))},this))},e.prototype.destroy=function(){this.$element.off(".owl.core"),this.$stage.off(".owl.core"),a(c).off(".owl.core"),!1!==this.settings.responsive&&(b.clearTimeout(this.resizeTimer),this.off(b,"resize",this._handlers.onThrottledResize));for(var d in this._plugins)this._plugins[d].destroy();this.$stage.children(".cloned").remove(),this.$stage.unwrap(),this.$stage.children().contents().unwrap(),this.$stage.children().unwrap(),this.$stage.remove(),this.$element.removeClass(this.options.refreshClass).removeClass(this.options.loadingClass).removeClass(this.options.loadedClass).removeClass(this.options.rtlClass).removeClass(this.options.dragClass).removeClass(this.options.grabClass).attr("class",this.$element.attr("class").replace(new RegExp(this.options.responsiveClass+"-\\S+\\s","g"),"")).removeData("owl.carousel")},e.prototype.op=function(a,b,c){var d=this.settings.rtl;switch(b){case"<":return d?a>c:a<c;case">":return d?a<c:a>c;case">=":return d?a<=c:a>=c;case"<=":return d?a>=c:a<=c}},e.prototype.on=function(a,b,c,d){a.addEventListener?a.addEventListener(b,c,d):a.attachEvent&&a.attachEvent("on"+b,c)},e.prototype.off=function(a,b,c,d){a.removeEventListener?a.removeEventListener(b,c,d):a.detachEvent&&a.detachEvent("on"+b,c)},e.prototype.trigger=function(b,c,d,f,g){var h={item:{count:this._items.length,index:this.current()}},i=a.camelCase(a.grep(["on",b,d],function(a){return a}).join("-").toLowerCase()),j=a.Event([b,"owl",d||"carousel"].join(".").toLowerCase(),a.extend({relatedTarget:this},h,c));return this._supress[b]||(a.each(this._plugins,function(a,b){b.onTrigger&&b.onTrigger(j)}),this.register({type:e.Type.Event,name:b}),this.$element.trigger(j),this.settings&&"function"==typeof this.settings[i]&&this.settings[i].call(this,j)),j},e.prototype.enter=function(b){a.each([b].concat(this._states.tags[b]||[]),a.proxy(function(a,b){this._states.current[b]===d&&(this._states.current[b]=0),this._states.current[b]++},this))},e.prototype.leave=function(b){a.each([b].concat(this._states.tags[b]||[]),a.proxy(function(a,b){this._states.current[b]--},this))},e.prototype.register=function(b){if(b.type===e.Type.Event){if(a.event.special[b.name]||(a.event.special[b.name]={}),!a.event.special[b.name].owl){var c=a.event.special[b.name]._default;a.event.special[b.name]._default=function(a){return!c||!c.apply||a.namespace&&-1!==a.namespace.indexOf("owl")?a.namespace&&a.namespace.indexOf("owl")>-1:c.apply(this,arguments)},a.event.special[b.name].owl=!0}}else b.type===e.Type.State&&(this._states.tags[b.name]?this._states.tags[b.name]=this._states.tags[b.name].concat(b.tags):this._states.tags[b.name]=b.tags,this._states.tags[b.name]=a.grep(this._states.tags[b.name],a.proxy(function(c,d){return a.inArray(c,this._states.tags[b.name])===d},this)))},e.prototype.suppress=function(b){a.each(b,a.proxy(function(a,b){this._supress[b]=!0},this))},e.prototype.release=function(b){a.each(b,a.proxy(function(a,b){delete this._supress[b]},this))},e.prototype.pointer=function(a){var c={x:null,y:null};return a=a.originalEvent||a||b.event,a=a.touches&&a.touches.length?a.touches[0]:a.changedTouches&&a.changedTouches.length?a.changedTouches[0]:a,a.pageX?(c.x=a.pageX,c.y=a.pageY):(c.x=a.clientX,c.y=a.clientY),c},e.prototype.isNumeric=function(a){return!isNaN(parseFloat(a))},e.prototype.difference=function(a,b){return{x:a.x-b.x,y:a.y-b.y}},a.fn.owlCarousel=function(b){var c=Array.prototype.slice.call(arguments,1);return this.each(function(){var d=a(this),f=d.data("owl.carousel");f||(f=new e(this,"object"==typeof b&&b),d.data("owl.carousel",f),a.each(["next","prev","to","destroy","refresh","replace","add","remove"],function(b,c){f.register({type:e.Type.Event,name:c}),f.$element.on(c+".owl.carousel.core",a.proxy(function(a){a.namespace&&a.relatedTarget!==this&&(this.suppress([c]),f[c].apply(this,[].slice.call(arguments,1)),this.release([c]))},f))})),"string"==typeof b&&"_"!==b.charAt(0)&&f[b].apply(f,c)})},a.fn.owlCarousel.Constructor=e}(window.Zepto||window.jQuery,window,document),function(a,b,c,d){var e=function(b){this._core=b,this._interval=null,this._visible=null,this._handlers={"initialized.owl.carousel":a.proxy(function(a){a.namespace&&this._core.settings.autoRefresh&&this.watch()},this)},this._core.options=a.extend({},e.Defaults,this._core.options),this._core.$element.on(this._handlers)};e.Defaults={autoRefresh:!0,autoRefreshInterval:500},e.prototype.watch=function(){this._interval||(this._visible=this._core.isVisible(),this._interval=b.setInterval(a.proxy(this.refresh,this),this._core.settings.autoRefreshInterval))},e.prototype.refresh=function(){this._core.isVisible()!==this._visible&&(this._visible=!this._visible,this._core.$element.toggleClass("owl-hidden",!this._visible),this._visible&&this._core.invalidate("width")&&this._core.refresh())},e.prototype.destroy=function(){var a,c;b.clearInterval(this._interval);for(a in this._handlers)this._core.$element.off(a,this._handlers[a]);for(c in Object.getOwnPropertyNames(this))"function"!=typeof this[c]&&(this[c]=null)},a.fn.owlCarousel.Constructor.Plugins.AutoRefresh=e}(window.Zepto||window.jQuery,window,document),function(a,b,c,d){var e=function(b){this._core=b,this._loaded=[],this._handlers={"initialized.owl.carousel change.owl.carousel resized.owl.carousel":a.proxy(function(b){if(b.namespace&&this._core.settings&&this._core.settings.lazyLoad&&(b.property&&"position"==b.property.name||"initialized"==b.type)){var c=this._core.settings,e=c.center&&Math.ceil(c.items/2)||c.items,f=c.center&&-1*e||0,g=(b.property&&b.property.value!==d?b.property.value:this._core.current())+f,h=this._core.clones().length,i=a.proxy(function(a,b){this.load(b)},this);for(c.lazyLoadEager>0&&(e+=c.lazyLoadEager,c.loop&&(g-=c.lazyLoadEager,e++));f++<e;)this.load(h/2+this._core.relative(g)),h&&a.each(this._core.clones(this._core.relative(g)),i),g++}},this)},this._core.options=a.extend({},e.Defaults,this._core.options),this._core.$element.on(this._handlers)};e.Defaults={lazyLoad:!1,lazyLoadEager:0},e.prototype.load=function(c){var d=this._core.$stage.children().eq(c),e=d&&d.find(".owl-lazy");!e||a.inArray(d.get(0),this._loaded)>-1||(e.each(a.proxy(function(c,d){var e,f=a(d),g=b.devicePixelRatio>1&&f.attr("data-src-retina")||f.attr("data-src")||f.attr("data-srcset");this._core.trigger("load",{element:f,url:g},"lazy"),f.is("img")?f.one("load.owl.lazy",a.proxy(function(){f.css("opacity",1),this._core.trigger("loaded",{element:f,url:g},"lazy")},this)).attr("src",g):f.is("source")?f.one("load.owl.lazy",a.proxy(function(){this._core.trigger("loaded",{element:f,url:g},"lazy")},this)).attr("srcset",g):(e=new Image,e.onload=a.proxy(function(){f.css({"background-image":'url("'+g+'")',opacity:"1"}),this._core.trigger("loaded",{element:f,url:g},"lazy")},this),e.src=g)},this)),this._loaded.push(d.get(0)))},e.prototype.destroy=function(){var a,b;for(a in this.handlers)this._core.$element.off(a,this.handlers[a]);for(b in Object.getOwnPropertyNames(this))"function"!=typeof this[b]&&(this[b]=null)},a.fn.owlCarousel.Constructor.Plugins.Lazy=e}(window.Zepto||window.jQuery,window,document),function(a,b,c,d){var e=function(c){this._core=c,this._previousHeight=null,this._handlers={"initialized.owl.carousel refreshed.owl.carousel":a.proxy(function(a){a.namespace&&this._core.settings.autoHeight&&this.update()},this),"changed.owl.carousel":a.proxy(function(a){a.namespace&&this._core.settings.autoHeight&&"position"===a.property.name&&this.update()},this),"loaded.owl.lazy":a.proxy(function(a){a.namespace&&this._core.settings.autoHeight&&a.element.closest("."+this._core.settings.itemClass).index()===this._core.current()&&this.update()},this)},this._core.options=a.extend({},e.Defaults,this._core.options),this._core.$element.on(this._handlers),this._intervalId=null;var d=this;a(b).on("load",function(){d._core.settings.autoHeight&&d.update()}),a(b).resize(function(){d._core.settings.autoHeight&&(null!=d._intervalId&&clearTimeout(d._intervalId),d._intervalId=setTimeout(function(){d.update()},250))})};e.Defaults={autoHeight:!1,autoHeightClass:"owl-height"},e.prototype.update=function(){var b=this._core._current,c=b+this._core.settings.items,d=this._core.settings.lazyLoad,e=this._core.$stage.children().toArray().slice(b,c),f=[],g=0;a.each(e,function(b,c){f.push(a(c).height())}),g=Math.max.apply(null,f),g<=1&&d&&this._previousHeight&&(g=this._previousHeight),this._previousHeight=g,this._core.$stage.parent().height(g).addClass(this._core.settings.autoHeightClass)},e.prototype.destroy=function(){var a,b;for(a in this._handlers)this._core.$element.off(a,this._handlers[a]);for(b in Object.getOwnPropertyNames(this))"function"!=typeof this[b]&&(this[b]=null)},a.fn.owlCarousel.Constructor.Plugins.AutoHeight=e}(window.Zepto||window.jQuery,window,document),function(a,b,c,d){var e=function(b){this._core=b,this._videos={},this._playing=null,this._handlers={"initialized.owl.carousel":a.proxy(function(a){a.namespace&&this._core.register({type:"state",name:"playing",tags:["interacting"]})},this),"resize.owl.carousel":a.proxy(function(a){a.namespace&&this._core.settings.video&&this.isInFullScreen()&&a.preventDefault()},this),"refreshed.owl.carousel":a.proxy(function(a){a.namespace&&this._core.is("resizing")&&this._core.$stage.find(".cloned .owl-video-frame").remove()},this),"changed.owl.carousel":a.proxy(function(a){a.namespace&&"position"===a.property.name&&this._playing&&this.stop()},this),"prepared.owl.carousel":a.proxy(function(b){if(b.namespace){var c=a(b.content).find(".owl-video");c.length&&(c.css("display","none"),this.fetch(c,a(b.content)))}},this)},this._core.options=a.extend({},e.Defaults,this._core.options),this._core.$element.on(this._handlers),this._core.$element.on("click.owl.video",".owl-video-play-icon",a.proxy(function(a){this.play(a)},this))};e.Defaults={video:!1,videoHeight:!1,videoWidth:!1},e.prototype.fetch=function(a,b){var c=function(){return a.attr("data-vimeo-id")?"vimeo":a.attr("data-vzaar-id")?"vzaar":"youtube"}(),d=a.attr("data-vimeo-id")||a.attr("data-youtube-id")||a.attr("data-vzaar-id"),e=a.attr("data-width")||this._core.settings.videoWidth,f=a.attr("data-height")||this._core.settings.videoHeight,g=a.attr("href");if(!g)throw new Error("Missing video URL.");if(d=g.match(/(http:|https:|)\/\/(player.|www.|app.)?(vimeo\.com|youtu(be\.com|\.be|be\.googleapis\.com|be\-nocookie\.com)|vzaar\.com)\/(video\/|videos\/|embed\/|channels\/.+\/|groups\/.+\/|watch\?v=|v\/)?([A-Za-z0-9._%-]*)(\&\S+)?/),d[3].indexOf("youtu")>-1)c="youtube";else if(d[3].indexOf("vimeo")>-1)c="vimeo";else{if(!(d[3].indexOf("vzaar")>-1))throw new Error("Video URL not supported.");c="vzaar"}d=d[6],this._videos[g]={type:c,id:d,width:e,height:f},b.attr("data-video",g),this.thumbnail(a,this._videos[g])},e.prototype.thumbnail=function(b,c){var d,e,f,g=c.width&&c.height?"width:"+c.width+"px;height:"+c.height+"px;":"",h=b.find("img"),i="src",j="",k=this._core.settings,l=function(c){e='<div class="owl-video-play-icon"></div>',d=k.lazyLoad?a("<div/>",{class:"owl-video-tn "+j,srcType:c}):a("<div/>",{class:"owl-video-tn",style:"opacity:1;background-image:url("+c+")"}),b.after(d),b.after(e)};if(b.wrap(a("<div/>",{class:"owl-video-wrapper",style:g})),this._core.settings.lazyLoad&&(i="data-src",j="owl-lazy"),h.length)return l(h.attr(i)),h.remove(),!1;"youtube"===c.type?(f="//img.youtube.com/vi/"+c.id+"/hqdefault.jpg",l(f)):"vimeo"===c.type?a.ajax({type:"GET",url:"//vimeo.com/api/v2/video/"+c.id+".json",jsonp:"callback",dataType:"jsonp",success:function(a){f=a[0].thumbnail_large,l(f)}}):"vzaar"===c.type&&a.ajax({type:"GET",url:"//vzaar.com/api/videos/"+c.id+".json",jsonp:"callback",dataType:"jsonp",success:function(a){f=a.framegrab_url,l(f)}})},e.prototype.stop=function(){this._core.trigger("stop",null,"video"),this._playing.find(".owl-video-frame").remove(),this._playing.removeClass("owl-video-playing"),this._playing=null,this._core.leave("playing"),this._core.trigger("stopped",null,"video")},e.prototype.play=function(b){var c,d=a(b.target),e=d.closest("."+this._core.settings.itemClass),f=this._videos[e.attr("data-video")],g=f.width||"100%",h=f.height||this._core.$stage.height();this._playing||(this._core.enter("playing"),this._core.trigger("play",null,"video"),e=this._core.items(this._core.relative(e.index())),this._core.reset(e.index()),c=a('<iframe frameborder="0" allowfullscreen mozallowfullscreen webkitAllowFullScreen ></iframe>'),c.attr("height",h),c.attr("width",g),"youtube"===f.type?c.attr("src","//www.youtube.com/embed/"+f.id+"?autoplay=1&rel=0&v="+f.id):"vimeo"===f.type?c.attr("src","//player.vimeo.com/video/"+f.id+"?autoplay=1"):"vzaar"===f.type&&c.attr("src","//view.vzaar.com/"+f.id+"/player?autoplay=true"),a(c).wrap('<div class="owl-video-frame" />').insertAfter(e.find(".owl-video")),this._playing=e.addClass("owl-video-playing"))},e.prototype.isInFullScreen=function(){var b=c.fullscreenElement||c.mozFullScreenElement||c.webkitFullscreenElement;return b&&a(b).parent().hasClass("owl-video-frame")},e.prototype.destroy=function(){var a,b;this._core.$element.off("click.owl.video");for(a in this._handlers)this._core.$element.off(a,this._handlers[a]);for(b in Object.getOwnPropertyNames(this))"function"!=typeof this[b]&&(this[b]=null)},a.fn.owlCarousel.Constructor.Plugins.Video=e}(window.Zepto||window.jQuery,window,document),function(a,b,c,d){var e=function(b){this.core=b,this.core.options=a.extend({},e.Defaults,this.core.options),this.swapping=!0,this.previous=d,this.next=d,this.handlers={"change.owl.carousel":a.proxy(function(a){a.namespace&&"position"==a.property.name&&(this.previous=this.core.current(),this.next=a.property.value)},this),"drag.owl.carousel dragged.owl.carousel translated.owl.carousel":a.proxy(function(a){a.namespace&&(this.swapping="translated"==a.type)},this),"translate.owl.carousel":a.proxy(function(a){a.namespace&&this.swapping&&(this.core.options.animateOut||this.core.options.animateIn)&&this.swap()},this)},this.core.$element.on(this.handlers)};e.Defaults={animateOut:!1,
animateIn:!1},e.prototype.swap=function(){if(1===this.core.settings.items&&a.support.animation&&a.support.transition){this.core.speed(0);var b,c=a.proxy(this.clear,this),d=this.core.$stage.children().eq(this.previous),e=this.core.$stage.children().eq(this.next),f=this.core.settings.animateIn,g=this.core.settings.animateOut;this.core.current()!==this.previous&&(g&&(b=this.core.coordinates(this.previous)-this.core.coordinates(this.next),d.one(a.support.animation.end,c).css({left:b+"px"}).addClass("animated owl-animated-out").addClass(g)),f&&e.one(a.support.animation.end,c).addClass("animated owl-animated-in").addClass(f))}},e.prototype.clear=function(b){a(b.target).css({left:""}).removeClass("animated owl-animated-out owl-animated-in").removeClass(this.core.settings.animateIn).removeClass(this.core.settings.animateOut),this.core.onTransitionEnd()},e.prototype.destroy=function(){var a,b;for(a in this.handlers)this.core.$element.off(a,this.handlers[a]);for(b in Object.getOwnPropertyNames(this))"function"!=typeof this[b]&&(this[b]=null)},a.fn.owlCarousel.Constructor.Plugins.Animate=e}(window.Zepto||window.jQuery,window,document),function(a,b,c,d){var e=function(b){this._core=b,this._call=null,this._time=0,this._timeout=0,this._paused=!0,this._handlers={"changed.owl.carousel":a.proxy(function(a){a.namespace&&"settings"===a.property.name?this._core.settings.autoplay?this.play():this.stop():a.namespace&&"position"===a.property.name&&this._paused&&(this._time=0)},this),"initialized.owl.carousel":a.proxy(function(a){a.namespace&&this._core.settings.autoplay&&this.play()},this),"play.owl.autoplay":a.proxy(function(a,b,c){a.namespace&&this.play(b,c)},this),"stop.owl.autoplay":a.proxy(function(a){a.namespace&&this.stop()},this),"mouseover.owl.autoplay":a.proxy(function(){this._core.settings.autoplayHoverPause&&this._core.is("rotating")&&this.pause()},this),"mouseleave.owl.autoplay":a.proxy(function(){this._core.settings.autoplayHoverPause&&this._core.is("rotating")&&this.play()},this),"touchstart.owl.core":a.proxy(function(){this._core.settings.autoplayHoverPause&&this._core.is("rotating")&&this.pause()},this),"touchend.owl.core":a.proxy(function(){this._core.settings.autoplayHoverPause&&this.play()},this)},this._core.$element.on(this._handlers),this._core.options=a.extend({},e.Defaults,this._core.options)};e.Defaults={autoplay:!1,autoplayTimeout:5e3,autoplayHoverPause:!1,autoplaySpeed:!1},e.prototype._next=function(d){this._call=b.setTimeout(a.proxy(this._next,this,d),this._timeout*(Math.round(this.read()/this._timeout)+1)-this.read()),this._core.is("interacting")||c.hidden||this._core.next(d||this._core.settings.autoplaySpeed)},e.prototype.read=function(){return(new Date).getTime()-this._time},e.prototype.play=function(c,d){var e;this._core.is("rotating")||this._core.enter("rotating"),c=c||this._core.settings.autoplayTimeout,e=Math.min(this._time%(this._timeout||c),c),this._paused?(this._time=this.read(),this._paused=!1):b.clearTimeout(this._call),this._time+=this.read()%c-e,this._timeout=c,this._call=b.setTimeout(a.proxy(this._next,this,d),c-e)},e.prototype.stop=function(){this._core.is("rotating")&&(this._time=0,this._paused=!0,b.clearTimeout(this._call),this._core.leave("rotating"))},e.prototype.pause=function(){this._core.is("rotating")&&!this._paused&&(this._time=this.read(),this._paused=!0,b.clearTimeout(this._call))},e.prototype.destroy=function(){var a,b;this.stop();for(a in this._handlers)this._core.$element.off(a,this._handlers[a]);for(b in Object.getOwnPropertyNames(this))"function"!=typeof this[b]&&(this[b]=null)},a.fn.owlCarousel.Constructor.Plugins.autoplay=e}(window.Zepto||window.jQuery,window,document),function(a,b,c,d){"use strict";var e=function(b){this._core=b,this._initialized=!1,this._pages=[],this._controls={},this._templates=[],this.$element=this._core.$element,this._overrides={next:this._core.next,prev:this._core.prev,to:this._core.to},this._handlers={"prepared.owl.carousel":a.proxy(function(b){b.namespace&&this._core.settings.dotsData&&this._templates.push('<div class="'+this._core.settings.dotClass+'">'+a(b.content).find("[data-dot]").addBack("[data-dot]").attr("data-dot")+"</div>")},this),"added.owl.carousel":a.proxy(function(a){a.namespace&&this._core.settings.dotsData&&this._templates.splice(a.position,0,this._templates.pop())},this),"remove.owl.carousel":a.proxy(function(a){a.namespace&&this._core.settings.dotsData&&this._templates.splice(a.position,1)},this),"changed.owl.carousel":a.proxy(function(a){a.namespace&&"position"==a.property.name&&this.draw()},this),"initialized.owl.carousel":a.proxy(function(a){a.namespace&&!this._initialized&&(this._core.trigger("initialize",null,"navigation"),this.initialize(),this.update(),this.draw(),this._initialized=!0,this._core.trigger("initialized",null,"navigation"))},this),"refreshed.owl.carousel":a.proxy(function(a){a.namespace&&this._initialized&&(this._core.trigger("refresh",null,"navigation"),this.update(),this.draw(),this._core.trigger("refreshed",null,"navigation"))},this)},this._core.options=a.extend({},e.Defaults,this._core.options),this.$element.on(this._handlers)};e.Defaults={nav:!1,navText:['<span aria-label="Previous">&#x2039;</span>','<span aria-label="Next">&#x203a;</span>'],navSpeed:!1,navElement:'button type="button" role="presentation"',navContainer:!1,navContainerClass:"owl-nav",navClass:["owl-prev","owl-next"],slideBy:1,dotClass:"owl-dot",dotsClass:"owl-dots",dots:!0,dotsEach:!1,dotsData:!1,dotsSpeed:!1,dotsContainer:!1},e.prototype.initialize=function(){var b,c=this._core.settings;this._controls.$relative=(c.navContainer?a(c.navContainer):a("<div>").addClass(c.navContainerClass).appendTo(this.$element)).addClass("disabled"),this._controls.$previous=a("<"+c.navElement+">").addClass(c.navClass[0]).html(c.navText[0]).prependTo(this._controls.$relative).on("click",a.proxy(function(a){this.prev(c.navSpeed)},this)),this._controls.$next=a("<"+c.navElement+">").addClass(c.navClass[1]).html(c.navText[1]).appendTo(this._controls.$relative).on("click",a.proxy(function(a){this.next(c.navSpeed)},this)),c.dotsData||(this._templates=[a('<button role="button">').addClass(c.dotClass).append(a("<span>")).prop("outerHTML")]),this._controls.$absolute=(c.dotsContainer?a(c.dotsContainer):a("<div>").addClass(c.dotsClass).appendTo(this.$element)).addClass("disabled"),this._controls.$absolute.on("click","button",a.proxy(function(b){var d=a(b.target).parent().is(this._controls.$absolute)?a(b.target).index():a(b.target).parent().index();b.preventDefault(),this.to(d,c.dotsSpeed)},this));for(b in this._overrides)this._core[b]=a.proxy(this[b],this)},e.prototype.destroy=function(){var a,b,c,d,e;e=this._core.settings;for(a in this._handlers)this.$element.off(a,this._handlers[a]);for(b in this._controls)"$relative"===b&&e.navContainer?this._controls[b].html(""):this._controls[b].remove();for(d in this.overides)this._core[d]=this._overrides[d];for(c in Object.getOwnPropertyNames(this))"function"!=typeof this[c]&&(this[c]=null)},e.prototype.update=function(){var a,b,c,d=this._core.clones().length/2,e=d+this._core.items().length,f=this._core.maximum(!0),g=this._core.settings,h=g.center||g.autoWidth||g.dotsData?1:g.dotsEach||g.items;if("page"!==g.slideBy&&(g.slideBy=Math.min(g.slideBy,g.items)),g.dots||"page"==g.slideBy)for(this._pages=[],a=d,b=0,c=0;a<e;a++){if(b>=h||0===b){if(this._pages.push({start:Math.min(f,a-d),end:a-d+h-1}),Math.min(f,a-d)===f)break;b=0,++c}b+=this._core.mergers(this._core.relative(a))}},e.prototype.draw=function(){var b,c=this._core.settings,d=this._core.items().length<=c.items,e=this._core.relative(this._core.current()),f=c.loop||c.rewind;this._controls.$relative.toggleClass("disabled",!c.nav||d),c.nav&&(this._controls.$previous.toggleClass("disabled",!f&&e<=this._core.minimum(!0)),this._controls.$next.toggleClass("disabled",!f&&e>=this._core.maximum(!0))),this._controls.$absolute.toggleClass("disabled",!c.dots||d),c.dots&&(b=this._pages.length-this._controls.$absolute.children().length,c.dotsData&&0!==b?this._controls.$absolute.html(this._templates.join("")):b>0?this._controls.$absolute.append(new Array(b+1).join(this._templates[0])):b<0&&this._controls.$absolute.children().slice(b).remove(),this._controls.$absolute.find(".active").removeClass("active"),this._controls.$absolute.children().eq(a.inArray(this.current(),this._pages)).addClass("active"))},e.prototype.onTrigger=function(b){var c=this._core.settings;b.page={index:a.inArray(this.current(),this._pages),count:this._pages.length,size:c&&(c.center||c.autoWidth||c.dotsData?1:c.dotsEach||c.items)}},e.prototype.current=function(){var b=this._core.relative(this._core.current());return a.grep(this._pages,a.proxy(function(a,c){return a.start<=b&&a.end>=b},this)).pop()},e.prototype.getPosition=function(b){var c,d,e=this._core.settings;return"page"==e.slideBy?(c=a.inArray(this.current(),this._pages),d=this._pages.length,b?++c:--c,c=this._pages[(c%d+d)%d].start):(c=this._core.relative(this._core.current()),d=this._core.items().length,b?c+=e.slideBy:c-=e.slideBy),c},e.prototype.next=function(b){a.proxy(this._overrides.to,this._core)(this.getPosition(!0),b)},e.prototype.prev=function(b){a.proxy(this._overrides.to,this._core)(this.getPosition(!1),b)},e.prototype.to=function(b,c,d){var e;!d&&this._pages.length?(e=this._pages.length,a.proxy(this._overrides.to,this._core)(this._pages[(b%e+e)%e].start,c)):a.proxy(this._overrides.to,this._core)(b,c)},a.fn.owlCarousel.Constructor.Plugins.Navigation=e}(window.Zepto||window.jQuery,window,document),function(a,b,c,d){"use strict";var e=function(c){this._core=c,this._hashes={},this.$element=this._core.$element,this._handlers={"initialized.owl.carousel":a.proxy(function(c){c.namespace&&"URLHash"===this._core.settings.startPosition&&a(b).trigger("hashchange.owl.navigation")},this),"prepared.owl.carousel":a.proxy(function(b){if(b.namespace){var c=a(b.content).find("[data-hash]").addBack("[data-hash]").attr("data-hash");if(!c)return;this._hashes[c]=b.content}},this),"changed.owl.carousel":a.proxy(function(c){if(c.namespace&&"position"===c.property.name){var d=this._core.items(this._core.relative(this._core.current())),e=a.map(this._hashes,function(a,b){return a===d?b:null}).join();if(!e||b.location.hash.slice(1)===e)return;b.location.hash=e}},this)},this._core.options=a.extend({},e.Defaults,this._core.options),this.$element.on(this._handlers),a(b).on("hashchange.owl.navigation",a.proxy(function(a){var c=b.location.hash.substring(1),e=this._core.$stage.children(),f=this._hashes[c]&&e.index(this._hashes[c]);f!==d&&f!==this._core.current()&&this._core.to(this._core.relative(f),!1,!0)},this))};e.Defaults={URLhashListener:!1},e.prototype.destroy=function(){var c,d;a(b).off("hashchange.owl.navigation");for(c in this._handlers)this._core.$element.off(c,this._handlers[c]);for(d in Object.getOwnPropertyNames(this))"function"!=typeof this[d]&&(this[d]=null)},a.fn.owlCarousel.Constructor.Plugins.Hash=e}(window.Zepto||window.jQuery,window,document),function(a,b,c,d){function e(b,c){var e=!1,f=b.charAt(0).toUpperCase()+b.slice(1);return a.each((b+" "+h.join(f+" ")+f).split(" "),function(a,b){if(g[b]!==d)return e=!c||b,!1}),e}function f(a){return e(a,!0)}var g=a("<support>").get(0).style,h="Webkit Moz O ms".split(" "),i={transition:{end:{WebkitTransition:"webkitTransitionEnd",MozTransition:"transitionend",OTransition:"oTransitionEnd",transition:"transitionend"}},animation:{end:{WebkitAnimation:"webkitAnimationEnd",MozAnimation:"animationend",OAnimation:"oAnimationEnd",animation:"animationend"}}},j={csstransforms:function(){return!!e("transform")},csstransforms3d:function(){return!!e("perspective")},csstransitions:function(){return!!e("transition")},cssanimations:function(){return!!e("animation")}};j.csstransitions()&&(a.support.transition=new String(f("transition")),a.support.transition.end=i.transition.end[a.support.transition]),j.cssanimations()&&(a.support.animation=new String(f("animation")),a.support.animation.end=i.animation.end[a.support.animation]),j.csstransforms()&&(a.support.transform=new String(f("transform")),a.support.transform3d=j.csstransforms3d())}(window.Zepto||window.jQuery,window,document);


// ========== owl.carousel2.thumbs - v0.1.8 
!function(a,b,c,d){"use strict";var e=function(b){this.owl=b,this._thumbcontent=[],this._identifier=0,this.owl_currentitem=this.owl.options.startPosition,this.$element=this.owl.$element,this._handlers={"prepared.owl.carousel":a.proxy(function(b){if(!b.namespace||!this.owl.options.thumbs||this.owl.options.thumbImage||this.owl.options.thumbsPrerendered||this.owl.options.thumbImage){if(b.namespace&&this.owl.options.thumbs&&this.owl.options.thumbImage){var c=a(b.content).find("img");this._thumbcontent.push(c)}}else a(b.content).find("[data-thumb]").attr("data-thumb")!==d&&this._thumbcontent.push(a(b.content).find("[data-thumb]").attr("data-thumb"))},this),"initialized.owl.carousel":a.proxy(function(a){a.namespace&&this.owl.options.thumbs&&(this.render(),this.listen(),this._identifier=this.owl.$element.data("slider-id"),this.setActive())},this),"changed.owl.carousel":a.proxy(function(a){a.namespace&&"position"===a.property.name&&this.owl.options.thumbs&&(this._identifier=this.owl.$element.data("slider-id"),this.setActive())},this)},this.owl.options=a.extend({},e.Defaults,this.owl.options),this.owl.$element.on(this._handlers)};e.Defaults={thumbs:!0,thumbImage:!1,thumbContainerClass:"owl-thumbs",thumbItemClass:"owl-thumb-item",moveThumbsInside:!1},e.prototype.listen=function(){var b=this.owl.options;b.thumbsPrerendered&&(this._thumbcontent._thumbcontainer=a("."+b.thumbContainerClass)),a(this._thumbcontent._thumbcontainer).on("click",this._thumbcontent._thumbcontainer.children(),a.proxy(function(c){this._identifier=a(c.target).closest("."+b.thumbContainerClass).data("slider-id");var d=a(c.target).parent().is(this._thumbcontent._thumbcontainer)?a(c.target).index():a(c.target).closest("."+b.thumbItemClass).index();b.thumbsPrerendered?a("[data-slider-id="+this._identifier+"]").trigger("to.owl.carousel",[d,b.dotsSpeed,!0]):this.owl.to(d,b.dotsSpeed),c.preventDefault()},this))},e.prototype.render=function(){var b=this.owl.options;b.thumbsPrerendered?(this._thumbcontent._thumbcontainer=a("."+b.thumbContainerClass),b.moveThumbsInside&&this._thumbcontent._thumbcontainer.appendTo(this.$element)):this._thumbcontent._thumbcontainer=a("<div>").addClass(b.thumbContainerClass).appendTo(this.$element);var c;if(b.thumbImage)for(c=0;c<this._thumbcontent.length;++c)this._thumbcontent._thumbcontainer.append("<button class="+b.thumbItemClass+'><img src="'+this._thumbcontent[c].attr("src")+'" alt="'+this._thumbcontent[c].attr("alt")+'" /></button>');else for(c=0;c<this._thumbcontent.length;++c)this._thumbcontent._thumbcontainer.append("<button class="+b.thumbItemClass+">"+this._thumbcontent[c]+"</button>")},e.prototype.setActive=function(){this.owl_currentitem=this.owl._current-this.owl._clones.length/2,this.owl_currentitem===this.owl._items.length&&(this.owl_currentitem=0);var b=this.owl.options,c=b.thumbsPrerendered?a("."+b.thumbContainerClass+'[data-slider-id="'+this._identifier+'"]'):this._thumbcontent._thumbcontainer;c.children().filter(".active").removeClass("active"),c.children().eq(this.owl_currentitem).addClass("active")},e.prototype.destroy=function(){var a,b;for(a in this._handlers)this.owl.$element.off(a,this._handlers[a]);for(b in Object.getOwnPropertyNames(this))"function"!=typeof this[b]&&(this[b]=null)},a.fn.owlCarousel.Constructor.Plugins.Thumbs=e}(window.Zepto||window.jQuery,window,document);


// ========== vanilla-lazyload@13.0.1 ==
!function(t,e){"object"==typeof exports&&"undefined"!=typeof module?module.exports=e():"function"==typeof define&&define.amd?define(e):(t=t||self).LazyLoad=e()}(this,(function(){"use strict";function t(){return(t=Object.assign||function(t){for(var e=1;e<arguments.length;e++){var n=arguments[e];for(var r in n)Object.prototype.hasOwnProperty.call(n,r)&&(t[r]=n[r])}return t}).apply(this,arguments)}var e="undefined"!=typeof window,n=e&&!("onscroll"in window)||"undefined"!=typeof navigator&&/(gle|ing|ro)bot|crawl|spider/i.test(navigator.userAgent),r=e&&"IntersectionObserver"in window,a=e&&"classList"in document.createElement("p"),o={elements_selector:"img",container:n||e?document:null,threshold:300,thresholds:null,data_src:"src",data_srcset:"srcset",data_sizes:"sizes",data_bg:"bg",data_poster:"poster",class_loading:"loading",class_loaded:"loaded",class_error:"error",load_delay:0,auto_unobserve:!0,callback_enter:null,callback_exit:null,callback_reveal:null,callback_loaded:null,callback_error:null,callback_finish:null,use_native:!1},i=function(t,e){var n,r=new t(e);try{n=new CustomEvent("LazyLoad::Initialized",{detail:{instance:r}})}catch(t){(n=document.createEvent("CustomEvent")).initCustomEvent("LazyLoad::Initialized",!1,!1,{instance:r})}window.dispatchEvent(n)},s=function(t,e){return t.getAttribute("data-"+e)},c=function(t,e,n){var r="data-"+e;null!==n?t.setAttribute(r,n):t.removeAttribute(r)},l=function(t){return"true"===s(t,"was-processed")},u=function(t,e){return c(t,"ll-timeout",e)},d=function(t){return s(t,"ll-timeout")},f=function(t){for(var e,n=[],r=0;e=t.children[r];r+=1)"SOURCE"===e.tagName&&n.push(e);return n},_=function(t,e,n){n&&t.setAttribute(e,n)},v=function(t,e){_(t,"sizes",s(t,e.data_sizes)),_(t,"srcset",s(t,e.data_srcset)),_(t,"src",s(t,e.data_src))},g={IMG:function(t,e){var n=t.parentNode;n&&"PICTURE"===n.tagName&&f(n).forEach((function(t){v(t,e)}));v(t,e)},IFRAME:function(t,e){_(t,"src",s(t,e.data_src))},VIDEO:function(t,e){f(t).forEach((function(t){_(t,"src",s(t,e.data_src))})),_(t,"poster",s(t,e.data_poster)),_(t,"src",s(t,e.data_src)),t.load()}},h=function(t,e){var n,r,a=e._settings,o=t.tagName,i=g[o];if(i)return i(t,a),e.loadingCount+=1,void(e._elements=(n=e._elements,r=t,n.filter((function(t){return t!==r}))));!function(t,e){var n=s(t,e.data_src),r=s(t,e.data_bg);n&&(t.style.backgroundImage='url("'.concat(n,'")')),r&&(t.style.backgroundImage=r)}(t,a)},m=function(t,e){a?t.classList.add(e):t.className+=(t.className?" ":"")+e},b=function(t,e){a?t.classList.remove(e):t.className=t.className.replace(new RegExp("(^|\\s+)"+e+"(\\s+|$)")," ").replace(/^\s+/,"").replace(/\s+$/,"")},p=function(t,e,n,r){t&&(void 0===r?void 0===n?t(e):t(e,n):t(e,n,r))},y=function(t,e,n){t.addEventListener(e,n)},E=function(t,e,n){t.removeEventListener(e,n)},w=function(t,e,n){E(t,"load",e),E(t,"loadeddata",e),E(t,"error",n)},I=function(t,e,n){var r=n._settings,a=e?r.class_loaded:r.class_error,o=e?r.callback_loaded:r.callback_error,i=t.target;b(i,r.class_loading),m(i,a),p(o,i,n),n.loadingCount-=1,0===n._elements.length&&0===n.loadingCount&&p(r.callback_finish,n)},k=function(t,e){var n=function n(a){I(a,!0,e),w(t,n,r)},r=function r(a){I(a,!1,e),w(t,n,r)};!function(t,e,n){y(t,"load",e),y(t,"loadeddata",e),y(t,"error",n)}(t,n,r)},A=["IMG","IFRAME","VIDEO"],L=function(t,e){var n=e._observer;z(t,e),n&&e._settings.auto_unobserve&&n.unobserve(t)},z=function(t,e,n){var r=e._settings;!n&&l(t)||(A.indexOf(t.tagName)>-1&&(k(t,e),m(t,r.class_loading)),h(t,e),function(t){c(t,"was-processed","true")}(t),p(r.callback_reveal,t,e))},O=function(t){var e=d(t);e&&(clearTimeout(e),u(t,null))},N=function(t,e,n){var r=n._settings;p(r.callback_enter,t,e,n),r.load_delay?function(t,e){var n=e._settings.load_delay,r=d(t);r||(r=setTimeout((function(){L(t,e),O(t)}),n),u(t,r))}(t,n):L(t,n)},C=function(t){return!!r&&(t._observer=new IntersectionObserver((function(e){e.forEach((function(e){return function(t){return t.isIntersecting||t.intersectionRatio>0}(e)?N(e.target,e,t):function(t,e,n){var r=n._settings;p(r.callback_exit,t,e,n),r.load_delay&&O(t)}(e.target,e,t)}))}),{root:(e=t._settings).container===document?null:e.container,rootMargin:e.thresholds||e.threshold+"px"}),!0);var e},x=["IMG","IFRAME"],M=function(t){return Array.prototype.slice.call(t)},R=function(t,e){return function(t){return t.filter((function(t){return!l(t)}))}(M(t||function(t){return t.container.querySelectorAll(t.elements_selector)}(e)))},T=function(t){var e=t._settings,n=e.container.querySelectorAll("."+e.class_error);M(n).forEach((function(t){b(t,e.class_error),function(t){c(t,"was-processed",null)}(t)})),t.update()},j=function(n,r){var a;this._settings=function(e){return t({},o,e)}(n),this.loadingCount=0,C(this),this.update(r),a=this,e&&window.addEventListener("online",(function(t){T(a)}))};return j.prototype={update:function(t){var e,r=this,a=this._settings;(this._elements=R(t,a),!n&&this._observer)?(function(t){return t.use_native&&"loading"in HTMLImageElement.prototype}(a)&&((e=this)._elements.forEach((function(t){-1!==x.indexOf(t.tagName)&&(t.setAttribute("loading","lazy"),z(t,e))})),this._elements=R(t,a)),this._elements.forEach((function(t){r._observer.observe(t)}))):this.loadAll()},destroy:function(){var t=this;this._observer&&(this._elements.forEach((function(e){t._observer.unobserve(e)})),this._observer=null),this._elements=null,this._settings=null},load:function(t,e){z(t,this,e)},loadAll:function(){var t=this;this._elements.forEach((function(e){L(e,t)}))}},e&&function(t,e){if(e)if(e.length)for(var n,r=0;n=e[r];r+=1)i(t,n);else i(t,e)}(j,window.lazyLoadOptions),j}));


// ========== session
!function(e){e.session={_id:null,_cookieCache:void 0,_init:function(){window.name||(window.name=Math.random()),this._id=window.name,this._initCache();var e=new RegExp(this._generatePrefix()+"=([^;]+);").exec(document.cookie);if(e&&document.location.protocol!==e[1])for(var t in this._clearSession(),this._cookieCache)try{window.sessionStorage.setItem(t,this._cookieCache[t])}catch(e){}document.cookie=this._generatePrefix()+"="+document.location.protocol+";path=/;expires="+new Date((new Date).getTime()+12e4).toUTCString()},_generatePrefix:function(){return"__session:"+this._id+":"},_initCache:function(){var e=document.cookie.split(";");for(var t in this._cookieCache={},e){var i=e[t].split("=");new RegExp(this._generatePrefix()+".+").test(i[0])&&i[1]&&(this._cookieCache[i[0].split(":",3)[2]]=i[1])}},_setFallback:function(e,t,i){var o=this._generatePrefix()+e+"="+t+"; path=/";return i&&(o+="; expires="+new Date(Date.now()+12e4).toUTCString()),document.cookie=o,this._cookieCache[e]=t,this},_getFallback:function(e){return this._cookieCache||this._initCache(),this._cookieCache[e]},_clearFallback:function(){for(var e in this._cookieCache)document.cookie=this._generatePrefix()+e+"=; path=/; expires=Thu, 01 Jan 1970 00:00:01 GMT;";this._cookieCache={}},_deleteFallback:function(e){document.cookie=this._generatePrefix()+e+"=; path=/; expires=Thu, 01 Jan 1970 00:00:01 GMT;",delete this._cookieCache[e]},get:function(e){return window.sessionStorage.getItem(e)||this._getFallback(e)},set:function(e,t,i){try{window.sessionStorage.setItem(e,t)}catch(e){}return this._setFallback(e,t,i||!1),this},delete:function(e){return this.remove(e)},remove:function(e){try{window.sessionStorage.removeItem(e)}catch(e){}return this._deleteFallback(e),this},_clearSession:function(){try{window.sessionStorage.clear()}catch(e){for(var t in window.sessionStorage)window.sessionStorage.removeItem(t)}},clear:function(){return this._clearSession(),this._clearFallback(),this}},e.session._init()}(jQuery);

function alertInput(errorArray, color) {
    errorArray.forEach(function(input){
        input.style.borderColor = color;
    })
}

// ========== Product Like: Add / Remove
function addFavoriteProduct(productId) {
    if (check_user != '') {
        Hura.Ajax.post('user', {
            action_type: 'like', 
            item_type : 'product', 
            item_id: productId
        }).then(function(response){
            
                
                $('.success-form .content-text').html('Thêm sản phẩm ưa thích thành công !');

                $(".success-form").show();

                setTimeout(function(){
                    $(".success-form").fadeOut();
                }, 1100);

                setTimeout(function(){
                    $('.success-form .content-text').html('Thêm sản phẩm vào giỏ hàng thành công !');
                }, 1500);
            
        });

    } else {
        Fancybox.show([{ src: "#js-popup-customer-like"}]);
    }        
}

function removeFavoriteProduct(productId) {
    Hura.Ajax.post('user', {
        action_type: 'remove-like', 
        item_type : 'product', 
        item_id: productId
    }).then(function(response){
        if (response.status != 'success') {
            alert('- Có lỗi xảy ra. Vui lòng tải lại trang và thử lại')
        }

        location.reload();
    });
}

function calculatePriceOff(price, normal_price) {
    var percent = 0;

    if (normal_price > price && price > 0) {
        percent = Math.ceil(100 - price * 100 / normal_price);
    }

    return percent;
}


function formatArticleTime(article_time) {
    if (article_time.includes('Hôm nay')) {
        var time    = new Date();
        var day     = (time.getDate() <= 9) ? '0' + time.getDate() : time.getDate();
        var month   = (time.getMonth()+1 <= 9) ? '0' + (time.getMonth()+1) : (time.getMonth()+1);
        var year    = time.getFullYear();            

        var html = day + '.' + month + '.' + year;
    } else {
        var day_month   = article_time.substring(0,5).replaceAll('-','.');
        var year        = article_time.substring(6,10);
        var html        = day_month + '.' + year;
    }
    return html;
}

function checkSummary(productSummary){
    var summary = [];
    if(productSummary){
        var splitSummary = productSummary.split("\n");

        splitSummary.forEach(function(value,item){
            if(item < 5 && 1 < 2) {
                summary.push('<div class="item">'+ splitSummary[item] +'</div>');
            }
        })
    }
    return summary.join('');
}

function checkKhuyenMai(specialOffer){
    var offer = [];
    if(specialOffer){
        var splitOffer = specialOffer.split("\n");            

        splitOffer.forEach(function(value,item){
            if (item < 5 && 1 < 2) {
                offer.push(`<div class="item">`+ splitOffer[item] +`</div>`);
            }
        })
    }
    return offer.join('');
}



function countDownToNextDay(holder) {
    const today = new Date();
    const tomorrow = new Date(today);
    tomorrow.setDate(tomorrow.getDate() + 1);
    tomorrow.setHours(0,0,0,0); // set hour

    var countDownDate = tomorrow.setDate(tomorrow.getDate() + 1);

    var x = setInterval(function() {
        var now = new Date().getTime();
        var distance = countDownDate - now;

        var days = Math.floor(distance / (1000 * 60 * 60 * 24));
        var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        var seconds = Math.floor((distance % (1000 * 60)) / 1000);

        if(hours <=9)   {
            hours = "<b>0" + hours + "</b>";
        } else {
            hours = '<b>' + hours.toString().substr(0,1) + hours.toString().substr(1) + '</b>'
        }
        if(minutes <=9) {
            minutes = "<b>0" + minutes + "</b>";
        } else {
            minutes = '<b>' + minutes.toString().substr(0,1) + minutes.toString().substr(1) + '</b>'
        }
        if(seconds <=9) {
            seconds = "<b>0" + seconds + "</b>";
        } else {
            seconds = '<b>' + seconds.toString().substr(0,1) + seconds.toString().substr(1) + '</b>'
        }
        var html = hours + minutes + seconds;
        
        $(holder).html(html);

    }, 1000);
}


function checkContentHeight(height) {
    $(".js-static-content").each(function(){
        var $row        = $(this);
        var $row_height = $row.height();
        
        if ($row_height > height) {
            $row.css("max-height", height);
            $row.addClass("bg-content");
            $(".js-showmore-button").css("display","block");
        }
    });
    
    $(".js-showmore-button").click(function(){
        $(this).parents(".js-static-container").find(".js-static-content").css("max-height","unset");
        $(this).parents(".js-static-container").find(".js-static-content").removeClass("bg-content");
        $(this).parents(".js-static-container").find(".js-showless-button").css({"display": "block", "margin-top": "15px"});
        $(this).hide();
        
        $('html, body').animate({
            scrollTop: $(this).parents(".js-static-container").find(".js-static-content").offset().top - 160
        },500);
    });

    $(".js-showless-button").click(function(){
        $(this).parents(".js-static-container").find(".js-static-content").css("max-height",height);
        $(this).parents(".js-static-container").find(".js-static-content").addClass("bg-content");
        $(this).parents(".js-static-container").find(".js-showmore-button").css("display","block");
        $(this).hide();

        $('html, body').animate({
            scrollTop: $(this).parents(".js-static-container").find(".js-static-content").offset().top - 160
        },800);
    });
}

function tooltip(){
    var w_tooltip = $("#tooltip").width();
    var h_tooltip = 0;
    var pad = 10;
    var x_mouse = 0; 
    var y_mouse = 0;
    var wrap_left = 0;
    var wrap_right = 0;
    var wrap_top = 0;
    var wrap_bottom = 0;

    $(".p-item .p-img").mousemove(function(e){
        content_tooltip = $(this).parents(".p-item").find(".p-tooltip");
        if(content_tooltip.length == 0){
            return;
            $("#tooltip").hide();
        }

        $("#tooltip").html(content_tooltip.html());

        wrap_left = 0;
        wrap_top = $(window).scrollTop();
        wrap_bottom = $(window).height();
        wrap_right = $(window).width();
        x_mouse = e.pageX;
        y_mouse = e.pageY;
        h_tooltip = $("#tooltip").height();

        if(x_mouse + w_tooltip > wrap_right) $("#tooltip").css("left",x_mouse - w_tooltip - pad);
        else $("#tooltip").css("left",x_mouse + pad);

        if(y_mouse - h_tooltip < wrap_top) $("#tooltip").css("top",wrap_top);
        else $("#tooltip").css("top",y_mouse - h_tooltip - pad);

        $("#tooltip").show();
    });

    $(".p-item .p-img").mouseout(function(){
        $("#tooltip").hide();
    });
}

function showCartSummary(display_node) {
    var $status_container = $(display_node);
    $status_container.html('...');
    Hura.Cart.getSummary().then(summary => {
          $status_container.html(summary.total_item);
    });
}


function check_login(error_holder){
    var error = "";
    var email = document.getElementById('js-login-email').value;
    if (email.length < 6) error += "- Mời bạn nhập địa chỉ email<br>";

    var password = document.getElementById('js-login-pass').value;
    if (password.length == 0 ) error += "- Bạn cần nhập mật khẩu <br>";

    if (error != "") {
        $(error_holder).html(error);
        return false;
    } else {
        Hura.User.login(email, password).then(function (data) {
            //console.log(data);
            if (data.status == 'error') {                
                $(error_holder).html(data.message);
            } else if (GetURLParameter('return_url')) {
                location.href =  GetURLParameter('return_url').replaceAll(';amp;','&').replaceAll(';equals;','=')
            } else {
              
                location.href = '/taikhoan'
            }
        });
    }
    
        
}

function check_field_registor(error_holder) {
    var ERROR_NOTE      = error_holder;
    var number_regex1   = /^[0]\d{9}$/i;
    var number_regex2   = /^[0]\d{10}$/i;
    var error           = "";
    var errorInput      = [];

    var email   = document.getElementById("email");   
    if(email.value.length < 4) {
        errorInput = [...errorInput, email];
        error += "- Bạn chưa nhập email<br>";            

    } else if(validateEmail(email.value)==false){
        errorInput = [...errorInput, email];
        error += "- Email không hợp lệ <br>";
    }

    var password = document.getElementById('password');
    if(password.value.length < 6) {
        errorInput = [...errorInput, password];
        error += "- Mật khẩu yếu<br>";            
    }

    var pass1   =  document.getElementById("password1");
    if (pass1.value != password.value) {
        errorInput = [...errorInput, pass1];
        error += '- Mật khẩu không trùng khớp. Vui lòng nhập lại <br>';
    }

    var full_name = document.getElementById('full_name');
    if(full_name.value.length < 2) {
        errorInput = [...errorInput, full_name];
        error += "- Tên quá ngắn<br>";            

    } else if(full_name.value.indexOf('<script') > -1) { 
        errorInput = [...errorInput, full_name];
        error += "- Họ tên chứa các ký tự không hợp lệ, bạn vui lòng kiểm tra lại<br>";
    } 

    var mobile = document.getElementById('tel');
    if(mobile.value.length < 4) {
        errorInput = [...errorInput, mobile];
        error += "- Bạn chưa nhập SĐT<br>";

    } else if(number_regex1.test(mobile.value) == false && number_regex2.test(mobile.value) == false){
        errorInput = [...errorInput, mobile];
        error += "- Số điện thoại chưa chính xác<br>";
    }

    var address = document.getElementById('address');
    if(address.value.length < 5){
        errorInput = [...errorInput, address];
         error += "- Địa chỉ quá ngắn<br>";

    } else if(address.value.indexOf('<script') > -1) { 
        errorInput = [...errorInput, address];
        error += "- Địa chỉ chứa các ký tự không hợp lệ, bạn vui lòng kiểm tra lại<br>";
    }


    var province = $("#ship_to_province option:checked").val();    
    var district = $("#js-district-holder option:checked").val();    
    var sex      = $('input[type="radio"]:checked').val()

    var setDefault      = [email, password, full_name, mobile, address, pass1]

    alertInput(setDefault, '#d9d9d9');
  
    if (error != "") {
        $(ERROR_NOTE).html(error);
        
        alertInput(errorInput, 'red');

        return false;
    } else {

        var registerParams = {
            action_type: "register",
            info : {
                email    : email.value,
                name     : full_name.value,
                tel      : mobile.value,
                mobile   : mobile.value, 
                sex      : 'male',
                birthday : '',
                password : password.value,
                address  : address.value,
                province : province,
                district : district
            }
        }

        Hura.Ajax.post('customer', registerParams).then(function (data) {
            console.log(data);
            if(data.status == 'error' && data.message == 'Email exist' ){

                $(ERROR_NOTE).html('Email đã được sử dụng <br> Vui lòng đăng ký lại ! ');

            } else {
                $(ERROR_NOTE).html('Bạn đã đăng ký thành công ! ');
              
                $('.customer-page button[type=button]').html("Đang xử lý ... !").css({"background":"#ccc", "border-color":"transparent", "pointer-events":"none"});
              
                location.href="/dang-nhap";
            }
        })

    }
}

function check_user_captcha(captcha){
    $('#check_captcha').html("... đang kiểm tra");

    var params = {
        captcha: captcha
    };

    Hura.Ajax.post('check-captcha', params).then(function (data) {
        console.log(data);
        $('#check_captcha').html(data);
    })
} 

function subscribe_newsletter(a, error_holder){
    var number_regex1   = /^[0]\d{9}$/i;
    var number_regex2   = /^[0]\d{10}$/i;
    var NOTIFY          = error_holder;
    var error           = "";
    var email           = $(a).val();

    if(email.length < 4) {
        error += "- Bạn chưa nhập email <br>";
    } else if(validateEmail(email)==false){
        error += "- Email không hợp lệ <br>";
    }

    if(error != "") {            
        $(NOTIFY).html(error).show();
        $(a).css('border','2px solid  #FA5252');
      
        return false;
    } else {
        var params = {
            action : 'customer', 
            action_type: 'register-newsletter', 
            info : { 
                full_name: 'Khách hàng nhận bản tin', 
                email: email
            }
        };

        $(NOTIFY).html("- Quý khách đã đăng ký thành công").show();
        $(a).css('border', 'unset');
        $(a).val("");

        setTimeout(function () {
            $(NOTIFY).html("").hide();
        }, 2000);
        //Hura.Ajax.post('customer', params).then(function (data) {
            
        //    if(data.status == 'success') {
        //        $(NOTIFY).html("- Quý khách đã đăng ký thành công").show();
        //        $(a).css('border','unset');
        //        $(a).val("");

        //        setTimeout(function(){
        //            $(NOTIFY).html("").hide();
        //        },2000);
                
        //    } else if(data.message == 'Email exist'){
        //        $(NOTIFY).html("- Email này đã tồn tại").show();
        //        $(a).css('border','2px solid  #FA5252');
        //    } else {

        //        $(NOTIFY).html("- Lỗi xảy ra, vui lòng thử lại").show();
        //        $(a).css('border','2px solid  #FA5252');
        //    }
        //})
    }
}

function show_datetime_from_unix(int){
    var date = new Date(int*1000);
    return date;
}

function toTimestamp(strDate){
    var datum = Date.parse(strDate);
    return datum/1000;
}

function countDown(time,iid) {
    var h = 12;
    var i = 0;
    var s = 0;
    var date = time;
    var arrDate = date.split("-");
    var dateFormat = arrDate[1].toString().trim()+"/"+arrDate[0].toString().trim()+"/"+arrDate[2].toString().trim();
      
    var time = (new Date(dateFormat)).getTime();
    var timeNow = (new Date()).getTime();
    var time_left =   parseInt(time)/1000 - parseInt(timeNow)/1000;         

    GetCount(time_left, iid)
}

function GetCount(ddate, iid) {
    amount = ddate //calc milliseconds between dates
    hours = 0;
    mins = 0;
    secs = 0;
    out = "";

    var days = Math.floor(amount / (60 * 60 * 24));    

    hours = Math.floor((amount  % ( 60 * 60 * 24)) / ( 60 * 60));

    //amount = amount % 3600;

    mins = Math.floor((amount  % ( 60 * 60)) / ( 60));

    //amount = amount % 60;

    secs = Math.floor(amount % 60);

    out +="<p><b> " + days + " </b><span>Ngày</span></p>";
    out +="<p><b> " + (hours <= 9 ? '0' : '') + hours + " </b><span>Giờ</span></p>";
    out +="<p><b> " + (mins <= 9 ? '0' : '') + mins + " </b><span>Phút</span></p>";
    out +="<p><b> " + (secs <= 9 ? '0' : '') + secs + " </b><span>Giây</span></p>";
    //out = out.substr(0, out.length - 2);
    
    $(iid).html(out)
    
    setTimeout(function() {
        GetCount(ddate-1, iid)
    }, 1000);
}

function formatDate(a){
    var a = new Date(parseInt(a)*1000);
    
    var year = a.getFullYear();
    var month = a.getMonth()+1;
    var date = a.getDate();
    var hour = a.getHours();
    var min = a.getMinutes();
    var sec = a.getSeconds();
    var time = date + '/' + month + '/' + year ;
    return time;
}

function GetURLParameter(sParam){
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++)
    {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] == sParam)
        {
            return sParameterName[1];
        }
    }
}

function validateEmail(sEmail) {
    var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    if (filter.test(sEmail)) {
        return true;
    }
    else {
        return false;
    }
}

function validatePhoneNumber(a){
    var number_regex1 = /^[0]\d{9}$/i;
    var number_regex2 = /^[0]\d{10}$/i;

    if(number_regex1.test(a) == false && number_regex2.test(a) == false) return false;
    return true;
}

function formatCurrency(a) {
    var b = parseFloat(a).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1.").toString();
    var len = b.length;
    b = b.substring(0, len - 3);
    return b;
}

function isOnScreen(elem) {
    // if the element doesn't exist, abort
    if( elem.length == 0 ) {
        return;
    }
    var $window = jQuery(window)
    var viewport_top = $window.scrollTop()
    var viewport_height = $window.height()
    var viewport_bottom = viewport_top + viewport_height
    var $elem = jQuery(elem)
    var top = $elem.offset().top
    var height = $elem.height()
    var bottom = top + height

    return  (top >= viewport_top && top < viewport_bottom) ||
            (bottom > viewport_top && bottom <= viewport_bottom) ||
            (height > viewport_height && top <= viewport_top && bottom >= viewport_bottom)
}

function strToNumber(str) {
    str += ''; //convert to str incase it's already a number
    while(str.indexOf(".") > 0){
        str = str.replace('.','');
    }
    var result = parseFloat(str);
    return isNaN(result) ? 0 : result;
}

function writeStringToPrice(str){
    str = (str+'').replace(/\./g, "");
    var first_group = str.substr(0,str.length % 3);
    var remain_group = str.replace(first_group,"");
    var num_group = remain_group.length/3;
    var result = "", group_of_three;

    for(var i=0;i < num_group;i++){
        group_of_three = remain_group.substr(i*3,3);
        result += group_of_three;
        if(i !== (num_group-1)) result += ".";
    }

    if(first_group.length > 0) {
        return (result !== "") ? first_group + "." + result : first_group;
    }

    return result;
}

// search
function _run_search() {
    var curr_text    = "";
    var count_select = 0;
    var curr_element = "";
    var textarea     = document.getElementById("js-global-search");

    $('#js-global-search').keyup(debounce(function(){
        inputString = $(this).val();
        search(inputString);
    },200));

    $("#js-global-search").keydown(function(e){
        var inputString = $("#js-global-search").val();
        if(e.keyCode == 13 && inputString == ''){
            return false;
        }
    });

    $('body').click(function(){
        $("#js-search-holder").hide();
    });
}

function search(inputString){
    var button = $('form[action="/tim"]').find('button');

    var params = {
        action_type : "search",
        limit       : 10,
        q           : inputString
    }

    Hura.Ajax.get('search', params).then(function (data) {        
        if (inputString.trim() != '') {  
            // console.log(data.list)
            if (data.list != '') {
                var html = Hura.Template.parse(instant_search_result_template, data.list);
                Hura.Template.render('#js-search-holder', html);

                button.attr('type','submit');

            } else {
                $('#js-search-holder').html('<p style="text-align: center;margin: 15px;">Không tìm thấy kết quả của <b>"'+inputString+'"</b></p>');
                button.attr('type','button');
            }
            
            $("#js-search-holder").show();
        } else {
            $("#js-search-holder").hide();
        }
    })
}

function debounce(func, wait, immediate) {
    var timeout;
    return function() {
        var context = this, args = arguments;
        var later = function() {
            timeout = null;
            if (!immediate) func.apply(context, args);
        };
        var callNow = immediate && !timeout;
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
        if (callNow) func.apply(context, args);
    };
};

function getTextAreaSelection(textarea) {
    var start = textarea.selectionStart, end = textarea.selectionEnd;
    return {
        start: start,
        end: end,
        length: end - start,
        text: textarea.value.slice(start, end)
    };
}

function detectPaste(textarea, callback) {
    textarea.onpaste = function() {
        var sel = getTextAreaSelection(textarea);
        var initialLength = textarea.value.length;
        window.setTimeout(function() {
            var val = textarea.value;
            var pastedTextLength = val.length - (initialLength - sel.length);
            var end = sel.start + pastedTextLength;
            callback({
                start: sel.start,
                end: end,
                length: pastedTextLength,
                text: val.slice(sel.start, end)
            });
        }, 1);
    };
}
// end search

function _getPartName(){
    var pathname = location.pathname;

    if(pathname == '/tim') {
        if ( GetURLParameter('q') ) {
            pathname = '/tim?q=' + GetURLParameter('q')
        } else {
            pathname = '/tim?q='
        }
    }

    $('.js-pathName').attr('href', pathname);
    $('.js-select-path').val(pathname);
}

function show_hide_pass(pa) {
    var x = $(pa).parents('.item').find('.input-pass').attr('type');
    
    if (x === "password") {
        $(pa).parents('.item').find('.input-pass').attr('type','text');
    } else {
        $(pa).parents('.item').find('.input-pass').attr('type','password');
    }
}

//function addProductToCart(product_id, quantity ,redirect) {
//    var product_prop = {
//        quantity: quantity,
//        buyer_note : ''
//    };

//    Hura.Cart.Product.add(product_id, 0, product_prop).then(function(response){
//        if (redirect == '/cart') {
//            location.href = redirect;
//        } else {
//            //showCartSummary(".js-cart-count");
			
//          	$('.js-cart-count').html(response.length);
          
//            $(".success-form").show();

//            setTimeout(function(){
//                $(".success-form").fadeOut(); 
//            }, 1000);
//        }
//    });
//}

// So sanh
function compare_addProduct(id,name,pro_url,img,pa) {
    var ERROR_ALERT = "#js-alert";
    var HOLDER      = "#js-compare-holder"
    var pro_img     = img;
    var pro_id      = id;
    
    if ($(HOLDER + " .compare-item").length < 3) {

        $(".global-compare-group").show();

        $(ERROR_ALERT).html("");
                                            
        if ($(pa).hasClass("selected") == true) {        
            $(HOLDER + " .compare-item").each(function(){
                var id = $(this).attr("data-id")
                if (id == pro_id) {
                    $(this).remove();
                }
            })
            $(pa).removeClass("selected");

        } else {
            var creat_item = `
                <div class="js-compare-item compare-item" data-id="`+pro_id+`">
                    <span class="remove-compare js-remove-compare" onclick="removeCompare(this)"></span>
                    <img src="`+ pro_img +`"/>
                    <a href="`+ pro_url +`" class="name" target="_blank">`+ name +`</a>
                </div>
            `;
            $("#js-compare-holder").append(creat_item);
            $(pa).addClass("selected");
        }

    } else {
        $(ERROR_ALERT).html("Bạn được chọn tối đa 3 sản phẩm !")

        if ($(pa).hasClass("selected") == true) {
            $(HOLDER + " .compare-item").each(function(){
                var id = $(this).attr("data-id")
                if (id == pro_id) {
                    $(this).remove();
                }
            })
            $(pa).removeClass("selected");
            $(ERROR_ALERT).html("");
        }
    }

    if ($(HOLDER + " .compare-item").length == 0){
        $(".global-compare-group").hide();
    }
    
}

function removeCompare(pa) {
    var id = $(pa).parents(".js-compare-item").attr("data-id");
    var pro_id = $("#js-pd-item").attr("data-id");
    $(pa).parents(".js-compare-item").remove();

    if(pro_id == id){
        $("#js-pd-item").removeClass("selected")
    }

    $('.js-p-compare').each(function(){
        var currentId = $(this).attr('data-id');

        if(currentId == id){
            $(this).removeClass("selected")
        }
    });
    
    if ($("#js-compare-holder .js-compare-item").length < 1) {
        $(".global-compare-group").hide();
    }
}

function compare_link(){
    var compare_item = $(".js-compare-item").length;
    if (compare_item < 2){
        $("#js-alert").html("Cần tối thiểu 2 sản phẩm để so sánh !")
    } else{
        $("#js-alert").html("")

        var ids = [];
        $(".js-compare-item").each(function(){
            var id = $(this).attr("data-id");
            ids.push(id) + ','
        })
        var current_list = String(ids);

        window.location = "/so-sanh?list=" + current_list;
    }
}

function compare_close() {
    $("#js-compare-holder").html("");
    $(".global-compare-group").hide();
    $(".js-p-compare").removeClass("selected")
}

function closePopupSearch() {
    $("#js-popup-seach").val("");
    $("#js-seach-holder").html("");
    $('.popup-search-container').hide();
}


function loaiTrungLap(array, key) {
    let check = {};
    let res = [];
    for(let i=0; i<array.length; i++) {
        if(!check[array[i][key]]){
            check[array[i][key]] = true;
            res.push(array[i]);
        }
    }
    return res;
}

// ====== COMMON ======
var Base64 = {

    // private property
    _keyStr : "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=",

    // public method for encoding
    encode : function (input) {
        var output = "";
        var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
        var i = 0;

        input = Base64._utf8_encode(input);

        while (i < input.length) {

            chr1 = input.charCodeAt(i++);
            chr2 = input.charCodeAt(i++);
            chr3 = input.charCodeAt(i++);

            enc1 = chr1 >> 2;
            enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
            enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
            enc4 = chr3 & 63;

            if (isNaN(chr2)) {
                enc3 = enc4 = 64;
            } else if (isNaN(chr3)) {
                enc4 = 64;
            }

            output = output +
                this._keyStr.charAt(enc1) + this._keyStr.charAt(enc2) +
                this._keyStr.charAt(enc3) + this._keyStr.charAt(enc4);

        }

        return output;
    },

    // public method for decoding
    decode : function (input) {
        var output = "";
        var chr1, chr2, chr3;
        var enc1, enc2, enc3, enc4;
        var i = 0;

        input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");

        while (i < input.length) {

            enc1 = this._keyStr.indexOf(input.charAt(i++));
            enc2 = this._keyStr.indexOf(input.charAt(i++));
            enc3 = this._keyStr.indexOf(input.charAt(i++));
            enc4 = this._keyStr.indexOf(input.charAt(i++));

            chr1 = (enc1 << 2) | (enc2 >> 4);
            chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
            chr3 = ((enc3 & 3) << 6) | enc4;

            output = output + String.fromCharCode(chr1);

            if (enc3 != 64) {
                output = output + String.fromCharCode(chr2);
            }
            if (enc4 != 64) {
                output = output + String.fromCharCode(chr3);
            }

        }

        output = Base64._utf8_decode(output);

        return output;

    },

    // private method for UTF-8 encoding
    _utf8_encode : function (string) {
        string = string.replace(/\r\n/g,"\n");
        var utftext = "";

        for (var n = 0; n < string.length; n++) {

            var c = string.charCodeAt(n);

            if (c < 128) {
                utftext += String.fromCharCode(c);
            }
            else if((c > 127) && (c < 2048)) {
                utftext += String.fromCharCode((c >> 6) | 192);
                utftext += String.fromCharCode((c & 63) | 128);
            }
            else {
                utftext += String.fromCharCode((c >> 12) | 224);
                utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                utftext += String.fromCharCode((c & 63) | 128);
            }

        }

        return utftext;
    },

    // private method for UTF-8 decoding
    _utf8_decode : function (utftext) {
        var string = "";
        var i = 0;
        var c = c1 = c2 = 0;

        while ( i < utftext.length ) {

            c = utftext.charCodeAt(i);

            if (c < 128) {
                string += String.fromCharCode(c);
                i++;
            }
            else if((c > 191) && (c < 224)) {
                c2 = utftext.charCodeAt(i+1);
                string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                i += 2;
            }
            else {
                c2 = utftext.charCodeAt(i+1);
                c3 = utftext.charCodeAt(i+2);
                string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                i += 3;
            }

        }

        return string;
    }

}

function checkBrowserEnableCookie(){
    var cookieEnabled=(navigator.cookieEnabled)?true:false
    if(typeof navigator.cookieEnabled=="undefined"&&!cookieEnabled){document.cookie="testcookie"
        cookieEnabled=(document.cookie.indexOf("testcookie")!=-1)?true:false}
    if(cookieEnabled)return true;else return false;
}

function hura_create_cookie(name,value,days){
    if(days){var date=new Date();date.setTime(date.getTime()+(days*24*60*60*1000));var expires="; expires="+date.toGMTString();}
    else var expires="";document.cookie=name+"="+Base64.encode(value)+expires+"; path=/;";
}

function hura_read_cookie(name){
    var nameEQ=name+"=";
    var ca=document.cookie.split(';');
    for(var i=0;i<ca.length;i++){
        var c=ca[i];
        while(c.charAt(0)==' ')c=c.substring(1,c.length);
        if(c.indexOf(nameEQ)==0) return Base64.decode(c.substring(nameEQ.length,c.length));
    }
    return null;
}

function hura_erase_cookie(name){
    hura_create_cookie(name,"",-1);
}    

function check_interger(quantity){
    var intRegex = /^\d+$/;
    if(intRegex.test(quantity)) {
        if(parseInt(quantity) > 0) return true;
        else return false;
    }
    return false;
}

// add product to history by time order (latest viewed first, if already viewed before, remove it before add to list)
function save_product_view_history(product_id){
    if(!check_interger(product_id)){
        return ;
    }

    var cookie_name = 'product_view_history';
    var cookie_value = hura_read_cookie(cookie_name);
    var current_list = (cookie_value != null) ? cookie_value.split(",") : [];

    // if id in the list, remove it
    var index = current_list.indexOf(product_id);
    if (index !== -1) current_list.splice(index, 1);

    // add it to the front
    current_list.unshift(product_id);

    // set cookie
    hura_create_cookie(cookie_name, current_list.join(","),30);
}

function remove_from_history(product_id){
    if(confirm("Bạn chắc chắn muốn xóa sản phẩm này ?")) {
        if(!check_interger(product_id)){
            return;
        }
        var cookie_name = 'product_view_history';
        var cookie_value = hura_read_cookie(cookie_name);
        var current_list = (cookie_value != null) ? cookie_value.split(",") : [];

        // if id in the list, remove it
        var index = current_list.indexOf(product_id);
        if (index !== -1) current_list.splice(index, 1);

        // set cookie
        hura_create_cookie(cookie_name, current_list.join(","),30);

        //refresh page
        window.location = window.location;
    }
}

//item_id : id cua noi dung can luu
function user_like_content(item_id, content, data_holder){
    $.post("/ajax/user_like.php", {item_id : item_id, content_type : content}, function(data) {
        if(data === 'error-not-login') {
            //show login form or alert user
            if(confirm("Bạn cần đăng nhập để sử dụng chức năng này. Click OK để đăng nhập")){
                window.location = "/dang-nhap?returnTo=" + window.location;
            }
        }else if(data === 'error') {
            alert("Có lỗi xảy ra !");
        }else{
            $("#"+data_holder).html(data);
            alert("Đã thêm sản phẩm yêu thích");
        }
    });
}

function user_vote_review(review_id, vote){
    $.post("/ajax/vote_product_review.php", {review_id : review_id, vote : vote}, function(data) {
        if(data == 'error-not-login') {
            //show login form or alert user
            if(confirm("Bạn cần đăng nhập để sử dụng chức năng này. Click OK để đăng nhập")){
                window.location = "/dang-nhap?return_url=" + window.location;
            }
        }else if(data == 'error-has-voted') {
            alert("Có lỗi xảy ra !");
        }else{

            if(vote == 'dislike') {
                var url = window.location;
                var review_url = encodeURI(url).replace("=review", "=write-review");
                if(review_url.search("=write-review") == -1) review_url += "?section=write-review";

                window.location = review_url;
                
            }else{
                alert("Cảm ơn bạn đã quan tâm");
            }
        }
    });
}


function buildRec(nodes, elm, lv) {
    var node;
    //console.log(nodes);
    // filter
    do {
        node = nodes.shift();
    } while(node && !(/^h[123]$/i.test(node.tagName)));
    // process the next node
    if(node) {
        var ul, li, cnt;
        var curLv = parseInt(node.tagName.substring(1));
        //console.log(curLv);
        var stt = 0;
        if(curLv == lv) { // same level append an il
            cnt = 0;
        } else if(curLv < lv) { // walk up then append il
            cnt = 0;
            do {
                elm = elm.parentNode.parentNode;
                cnt--;
            } while(cnt > (curLv - lv));
        } else if(curLv > lv) { // create children then append il
            cnt = 0;
            do {
                li = elm.lastChild;
                if(li == null)
                    li = elm.appendChild(document.createElement("li"));
                    elm = li.appendChild(document.createElement("ol"));
                    cnt++;
            } while(cnt < (curLv - lv));
        }

        li = elm.appendChild(document.createElement("li"));
        // replace the next line with archor tags or whatever you want
        li.innerHTML = '<a>'+ node.innerHTML+'</a>';
        // recursive call
        buildRec(nodes, elm, lv + cnt);
    }
}

function convertToSlug(Text){
    // var newText = Text.split('.').join(" ");
    if(Text.lastIndexOf(".") > 0 ) {
        var newText =  Text.substr(Text.lastIndexOf(".") + 2); 
        // console.log(newText);
    } else newText = Text;
    return newText
        .toLowerCase()
        .replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a")
        .replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e")
        .replace(/ì|í|ị|ỉ|ĩ/g, "i")
        .replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o")
        .replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u")
        .replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y")
        .replace(/đ/g, "d")
        .replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, "")
        .replace(/\u02C6|\u0306|\u031B/g, "")
        .replace(/[^\w ]+/g,'')
        .replace(/ +/g,'-')
    ;
} 