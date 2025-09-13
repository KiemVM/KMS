$(document).ready(() => {
    const minimize = $.cookie('data-kt-app-sidebar-minimize');
    if (minimize) {
        const body = document.querySelector('#kt_body');
        if (body) {
            body.setAttribute('data-kt-app-sidebar-minimize', minimize);
        }
    }
    const btnToggler = document.querySelector('#js-btn-aside-toggle');
    if (btnToggler) {
        btnToggler.addEventListener('toggle', (event) => {
            console.log(event)
            if (btnToggler.classList.contains('active')) {
                $.cookie('data-kt-app-sidebar-minimize', 'on');
            } else {
                $.cookie('data-kt-app-sidebar-minimize', 'off');
            }
        });
    }
    initSummernoteGlobalStyles();
});
function UploadImageCore(image, id) {
    let formData = new FormData();
    formData.append("files", image);
    console.log(image)
    window.objAjax.parameters = formData;
    AjaxFile(ConstUrl.WebCrm + "/api/file/upload-files?path=" + ProjectName + "/Summernote/Upload&extensions=docx,doc,pdf,png,jpg,mp4,avi,gif,tiff,xls,xlsx&maxsize=10240", window.objAjax,
        function (response) {
            console.log(response)
            if (response && response.length > 0)
                $(id).summernote("insertImage", response[0].Url);
        });
}

function addProductEmbedButton(strId) {
    $.summernote.ui.button({
        contents: '<i class="fa fa-shopping-cart"></i>',
        tooltip: 'Nhúng sản phẩm',
        click: function () {
            // Open product selection modal
            openProductEmbedModal(strId);
        }
    }).render();
}

// 2. Add product embed modal (simple version)
function openProductEmbedModal(strId) {
    $(strId).summernote('saveRange');
    if (!$('#productEmbedModal').length) {
        $('body').append(`
            <div class="modal fade" id="productEmbedModal" tabindex="-1">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">Chọn sản phẩm để nhúng</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                        </div>
                        <div class="modal-body">
                            <input type="text" id="product-embed-search" class="form-control mb-2" placeholder="Tìm sản phẩm...">
                            <div id="product-list-embed"></div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Đóng</button>
                        </div>
                    </div>
                </div>
            </div>
        `);

        // Search event
        $(document).on('input', '#product-embed-search', function () {
            loadProductEmbedList(strId, $(this).val());
        });
    }
    loadProductEmbedList(strId, '');
    $('#productEmbedModal').modal('show');
}

function loadProductEmbedList(strId, search) {
    $.get('/api/product/for-embed?q=' + encodeURIComponent(search || ''), function (products) {
        let html = products.map(p => `
            <div class="product-embed-item" style="display:flex;align-items:center;margin-bottom:10px;">
                <img src="${p.imageUrl}" style="width:60px;height:60px;object-fit:cover;margin-right:10px;">
                <div>
                    <a href="${p.link}" target="_blank">${p.name}</a>
                    <button class="btn btn-sm btn-primary ms-2" onclick="insertProductToEditor('${strId}', '${p.name}', '${p.imageUrl}', '${p.link}')">Nhúng</button>
                </div>
            </div>
        `).join('');
        $('#product-list-embed').html(html);
    });
}

// 3. Insert product info into editor
function insertProductToEditor(strId, name, imageUrl, link) {
    const html = `
       
            <a href="${link}" target="_blank">
                <img src="${imageUrl}" alt="${name}" style="width:80px;height:80px;object-fit:cover;margin-right:10px;">
                <p>${name}</p>
            </a>
            
    `;
    $(strId).summernote('restoreRange');
    $(strId).summernote('pasteHTML', html);
    $('#productEmbedModal').modal('hide');
}

function DeleteMedia(media) {
    if (!media) {
        console.error('Media not found!');
        return;
    }
    window.objAjax.parameters = {
        media
    };
    AjaxPostAsync(ConstUrl.WebCrm + `/api/file/summernote-remove-media`, window.objAjax, () => {
        console.info('Remove media success!');
    });
}

function SummernoteCore(strId, height) {
    debugger;
    if (height === undefined) height = 100;

    $(strId).summernote({
        lang: 'vi-VN',
        focus: true,
        height: height,
        tabsize: 2,
        fontSizes: ['8', '10', '12', '14', '16', '18', '24', '36', '48', '64', '82', '150'],
        colors: [
            ['#000000', '#424242', '#636363', '#9C9C94', '#CEC6CE', '#EFEFEF', '#F7F7F7', '#FFFFFF'],
            ['#ff0000', '#00ff00', '#0000ff', '#ffff00', '#00ffff', '#ff00ff']
        ],
        toolbar: [
            ['style', ['bold', 'italic', 'underline', 'clear']],
            ['font', ['strikethrough', 'superscript', 'subscript']],
            ['fontsize', ['fontsize']],
            ['color', ['color']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['align', ['alignLeft', 'alignCenter', 'alignRight', 'alignJustify']], // Thêm căn chỉnh
            ['insert', ['link', 'picture', 'video', 'productEmbed']],
            ['height', ['height']],
            ['view', ['fullscreen', 'codeview']]
        ],

        // Thêm các nút căn chỉnh tùy chỉnh
        buttons: {
            alignLeft: function (context) {
                var ui = $.summernote.ui;
                return ui.button({
                    contents: '<i class="fa fa-align-left"></i>',
                    tooltip: 'Căn trái',
                    click: function () {
                        context.invoke('editor.justifyLeft');
                    }
                }).render();
            },
            alignCenter: function (context) {
                var ui = $.summernote.ui;
                return ui.button({
                    contents: '<i class="fa fa-align-center"></i>',
                    tooltip: 'Căn giữa',
                    click: function () {
                        context.invoke('editor.justifyCenter');
                    }
                }).render();
            },
            alignRight: function (context) {
                var ui = $.summernote.ui;
                return ui.button({
                    contents: '<i class="fa fa-align-right"></i>',
                    tooltip: 'Căn phải',
                    click: function () {
                        context.invoke('editor.justifyRight');
                    }
                }).render();
            },
            alignJustify: function (context) {
                var ui = $.summernote.ui;
                return ui.button({
                    contents: '<i class="fa fa-align-justify"></i>',
                    tooltip: 'Căn đều',
                    click: function () {
                        context.invoke('editor.justifyFull');
                    }
                }).render();
            },
            productEmbed: function (context) {
                var ui = $.summernote.ui;
                return ui.button({
                    contents: '<i class="fa fa-shopping-cart"></i>',
                    tooltip: 'Nhúng sản phẩm',
                    click: function () {
                        // Open product selection modal
                        openProductEmbedModal(strId);
                    }
                }).render();
                
            }
        },

        callbacks: {
            onInit: function () {
                // Thêm CSS cho các icon căn chỉnh nếu chưa có
                if (!$('#summernote-alignment-css').length) {
                    $('<style id="summernote-alignment-css">')
                        .html(`
                            .note-toolbar .fa-align-left:before { content: "\\f036"; }
                            .note-toolbar .fa-align-center:before { content: "\\f037"; }
                            .note-toolbar .fa-align-right:before { content: "\\f038"; }
                            .note-toolbar .fa-align-justify:before { content: "\\f039"; }
                        `)
                        .appendTo('head');
                }
                debugger;
                // Thêm modal video tùy chỉnh
                addVideoModal(strId);
            }

           

            
        }
    });
}

// Hàm thêm modal video tùy chỉnh
function addVideoModal(strId) {
    var modalId = 'videoModal_' + strId.replace('#', '');

    // Tạo modal cho video nếu chưa có
    if (!$('#' + modalId).length) {
        var videoModal = `
            <div class="modal fade" id="${modalId}" tabindex="-1">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">Chèn Video</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                        </div>
                        <div class="modal-body">
                            <div class="mb-3">
                                <label class="form-label">URL Video:</label>
                                <input type="url" class="form-control" id="videoUrl_${modalId}" 
                                       placeholder="https://www.youtube.com/watch?v=... hoặc https://vimeo.com/...">
                                <small class="form-text text-muted">Hỗ trợ YouTube, Tiktok, Vimeo, và video MP4 trực tiếp</small>
                            </div>
                            <div class="mb-3">
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" id="autoplayVideo_${modalId}" checked>
                                    <label class="form-check-label" for="autoplayVideo_${modalId}">
                                        Tự động phát video (muted)
                                    </label>
                                </div>
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" id="loopVideo_${modalId}" checked>
                                    <label class="form-check-label" for="loopVideo_${modalId}">
                                        Lặp lại video
                                    </label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <label class="form-label">Chiều rộng:</label>
                                    <input type="number" class="form-control" id="videoWidth_${modalId}" value="560" min="200" max="1200">
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Chiều cao:</label>
                                    <input type="number" class="form-control" id="videoHeight_${modalId}" value="315" min="150" max="800">
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Hủy</button>
                            <button type="button" class="btn btn-primary" onclick="insertVideoFromCustomModal('${strId}', '${modalId}')">Chèn Video</button>
                        </div>
                    </div>
                </div>
            </div>
        `;

        $('body').append(videoModal);
    }

    // Override sự kiện click của nút video
    setTimeout(function () {
        $(strId).next('.note-editor').find('button[aria-label="Video"]').off('click').on('click', function (e) {
            $(strId).summernote('saveRange');
            e.preventDefault();
            $('#note-modal').modal('hide');
            $('#' + modalId).modal('show');
        });

    }, 100);
}

// Hàm chèn video từ modal tùy chỉnh - phiên bản cải thiện
function insertVideoFromCustomModal(strId, modalId) {
    var url = $('#videoUrl_' + modalId).val().trim();
    var width = $('#videoWidth_' + modalId).val() || 560;
    var height = $('#videoHeight_' + modalId).val() || 315;
    var autoplay = $('#autoplayVideo_' + modalId).is(':checked');
    var loop = $('#loopVideo_' + modalId).is(':checked');

    if (!url) {
        alert('Vui lòng nhập URL video');
        return;
    }

    var videoHtml = '';

    // YouTube regex (cải thiện)
    var youtubeRegex = /(?:youtube\.com\/(?:[^\/]+\/.+\/|(?:v|e(?:mbed)?)\/|.*[?&]v=)|youtu\.be\/)([^"&?\/\s]{11})/;
    var youtubeMatch = url.match(youtubeRegex);

    // Vimeo regex
    var vimeoRegex = /(?:vimeo\.com\/)([0-9]+)/;
    var vimeoMatch = url.match(vimeoRegex);

    // TikTok regex - cải thiện để handle nhiều format hơn
    var tiktokRegex = /(?:(?:https?:\/\/)?(?:www\.)?(?:vm\.)?tiktok\.com\/|(?:https?:\/\/)?(?:www\.)?tiktok\.com\/@[\w.-]+\/video\/)([0-9]+)/;
    var tiktokMatch = url.match(tiktokRegex);

    // TikTok short URL format (vm.tiktok.com)
    var tiktokShortRegex = /(?:vm\.tiktok\.com\/)([A-Za-z0-9]+)/;
    var tiktokShortMatch = url.match(tiktokShortRegex);

    if (youtubeMatch) {
        // YouTube embed (giữ nguyên code cũ)
        var videoId = youtubeMatch[1];
        var params = [];
        if (autoplay) { params.push('autoplay=1'); params.push('mute=1'); }
        if (loop) { params.push('loop=1'); params.push('playlist=' + videoId); }
        params.push('rel=0');
        params.push('modestbranding=1');
        var paramString = params.length > 0 ? '?' + params.join('&') : '';
        videoHtml = `<p>
            <iframe class="note-video-clip" src="https://www.youtube.com/embed/${videoId}${paramString}" 
                width="${width}" height="${height}" 
                frameborder="0" allowfullscreen allow="autoplay; encrypted-media"></iframe>
        </p>`;
    }
    else if (vimeoMatch) {
        // Vimeo embed (giữ nguyên code cũ)
        var videoId = vimeoMatch[1];
        var params = [];
        if (autoplay) { params.push('autoplay=1'); params.push('muted=1'); }
        if (loop) { params.push('loop=1'); }
        var paramString = params.length > 0 ? '?' + params.join('&') : '';
        videoHtml = `<p>
            <iframe class="note-video-clip" src="https://player.vimeo.com/video/${videoId}${paramString}" 
                width="${width}" height="${height}" 
                frameborder="0" allowfullscreen allow="autoplay; encrypted-media"></iframe>
        </p>`;
    }
    else if (tiktokMatch || tiktokShortMatch) {
        // TikTok embed - cải thiện
        var videoId = tiktokMatch ? tiktokMatch[1] : tiktokShortMatch[1];

        // Chuẩn hóa URL TikTok
        var cleanUrl = url.split('?')[0]; // Loại bỏ query parameters

        // Tạo unique ID cho mỗi video TikTok
        var uniqueId = 'tiktok_' + Date.now() + '_' + Math.random().toString(36).substr(2, 9);

        videoHtml = `<div class="tiktok-embed-container" style="max-width: 100%; margin: 20px auto; text-align: center;">
            <blockquote class="tiktok-embed" 
                cite="${cleanUrl}" 
                data-video-id="${videoId}" 
                data-unique-id="${uniqueId}"
                style="max-width: 605px; min-width: 325px; margin: 0 auto; display: block;">
                <section>
                    <a target="_blank" title="@tiktok" href="${cleanUrl}">
                        Xem video TikTok này
                    </a>
                </section>
            </blockquote>
        </div>`;

        // Load TikTok script nếu chưa có
        loadTikTokScript();
    }
    else {
        // Video MP4 hoặc link trực tiếp (giữ nguyên code cũ)
        var videoAttributes = 'controls';
        if (autoplay) videoAttributes += ' autoplay muted';
        if (loop) videoAttributes += ' loop';
        videoHtml = `<video ${videoAttributes} width="${width}" height="${height}" style="max-width: 100%;">
            <source src="${url}" type="video/mp4">
            <source src="${url}" type="video/webm">
            <source src="${url}" type="video/ogg">
            Trình duyệt của bạn không hỗ trợ thẻ video.
        </video>`;
    }

    if (!videoHtml) {
        alert('URL không hợp lệ hoặc không được hỗ trợ!');
        return;
    }

    // Chèn video vào editor tại vị trí con trỏ
    $(strId).summernote('restoreRange');
    $(strId).summernote('pasteHTML', videoHtml);

    // Đóng modal và reset form
    $('#' + modalId).modal('hide');
    $('#videoUrl_' + modalId).val('');
    $('#videoWidth_' + modalId).val('560');
    $('#videoHeight_' + modalId).val('315');
    $('#autoplayVideo_' + modalId).prop('checked', true);
    $('#loopVideo_' + modalId).prop('checked', true);

    // Render TikTok videos sau khi insert
    if (tiktokMatch || tiktokShortMatch) {
        setTimeout(() => {
            renderTikTokVideos();
        }, 100);
    }
}

// Hàm load TikTok embed script
function loadTikTokScript() {
    if (!window.tiktokEmbedScriptLoaded && !document.querySelector('script[src*="tiktok.com/embed.js"]')) {
        const script = document.createElement('script');
        script.async = true;
        script.src = 'https://www.tiktok.com/embed.js';
        script.onload = function () {
            window.tiktokEmbedScriptLoaded = true;
            renderTikTokVideos();
        };
        document.head.appendChild(script);
    } else if (window.tiktokEmbedScriptLoaded) {
        renderTikTokVideos();
    }
}

// Hàm render TikTok videos
function renderTikTokVideos() {
    if (typeof tiktokEmbed !== 'undefined' && tiktokEmbed.lib) {
        try {
            tiktokEmbed.lib.render();
        } catch (e) {
            console.warn('TikTok embed render failed:', e);
        }
    }
}

// Cải thiện hàm cleanSummernoteData để xử lý TikTok
function cleanSummernoteData(htmlContent) {
    if (!htmlContent) return '';

    var tempDiv = $('<div>').html(htmlContent);

    // ... (giữ nguyên code xử lý khác)

    // Xử lý TikTok embeds
    tempDiv.find('.tiktok-embed-container').each(function () {
        var container = $(this);
        // Đảm bảo TikTok embed responsive
        container.css({
            'max-width': '100%',
            'margin': '20px auto',
            'text-align': 'center'
        });

        // Đảm bảo blockquote TikTok có style phù hợp
        container.find('.tiktok-embed').css({
            'max-width': '605px',
            'min-width': '325px',
            'margin': '0 auto',
            'display': 'block'
        });
    });

    // ... (tiếp tục với code xử lý khác)

    return tempDiv.html();
}

// Thêm CSS cho TikTok embed trong initSummernoteGlobalStyles
function initSummernoteGlobalStyles() {
    if (!$('#summernote-global-styles').length) {
        $('<style id="summernote-global-styles">')
            .html(`
                /* Existing styles... */
                
                /* TikTok embed styles */
                .tiktok-embed-container {
                    max-width: 100% !important;
                    margin: 20px auto !important;
                    text-align: center !important;
                }
                
                .tiktok-embed {
                    max-width: 605px !important;
                    min-width: 325px !important;
                    margin: 0 auto !important;
                    display: block !important;
                }
                
                /* Responsive TikTok */
                @media (max-width: 768px) {
                    .tiktok-embed {
                        max-width: 100% !important;
                        min-width: 280px !important;
                    }
                }
                
                /* Loading state for TikTok */
                .tiktok-embed:not(.tiktok-embed-rendered) {
                    background: #f0f0f0;
                    border: 1px solid #ddd;
                    border-radius: 8px;
                    padding: 20px;
                    text-align: center;
                    color: #666;
                }
                
                .tiktok-embed:not(.tiktok-embed-rendered):before {
                    content: "Đang tải video TikTok...";
                    display: block;
                    font-size: 14px;
                }
            `)
            .appendTo('head');
    }
}

// Hàm khởi tạo observer để tự động render TikTok khi có thay đổi DOM
function initTikTokObserver() {
    if (typeof MutationObserver !== 'undefined') {
        const observer = new MutationObserver((mutations) => {
            let shouldRender = false;
            mutations.forEach((mutation) => {
                if (mutation.type === 'childList') {
                    mutation.addedNodes.forEach((node) => {
                        if (node.nodeType === 1 && node.querySelector && node.querySelector('.tiktok-embed')) {
                            shouldRender = true;
                        }
                    });
                }
            });

            if (shouldRender && window.tiktokEmbedScriptLoaded) {
                setTimeout(renderTikTokVideos, 100);
            }
        });

        // Observe document body for changes
        observer.observe(document.body, {
            childList: true,
            subtree: true
        });
    }
}

// Hàm làm sạch và tiền xử lý dữ liệu Summernote
function cleanSummernoteData(htmlContent) {
    if (!htmlContent) return '';

    // Tạo element tạm để xử lý
    var tempDiv = $('<div>').html(htmlContent);

    // 1. Loại bỏ các thuộc tính style không mong muốn
    tempDiv.find('*').each(function () {
        var element = $(this);
        var style = element.attr('style');

        if (style) {
            // Loại bỏ width cố định
            style = style.replace(/width:\s*[^;]+;?/gi, '');

            // Loại bỏ các thuộc tính có thể gây tràn
            style = style.replace(/margin-top:\s*[^;]+;?/gi, '');
            style = style.replace(/margin-bottom:\s*[^;]+;?/gi, '');
            style = style.replace(/font-family:\s*[^;]+;?/gi, '');
            style = style.replace(/font-size:\s*[^;]+;?/gi, '');
            style = style.replace(/color:\s*rgb\([^)]+\);?/gi, '');
            style = style.replace(/background-color:\s*rgb\([^)]+\);?/gi, '');
            style = style.replace(/text-decoration[^;]*;?/gi, '');
            style = style.replace(/box-sizing:\s*[^;]+;?/gi, '');
            style = style.replace(/text-rendering:\s*[^;]+;?/gi, '');
            style = style.replace(/line-height:\s*[^;]+;?/gi, '');
            style = style.replace(/letter-spacing:\s*[^;]+;?/gi, '');
            style = style.replace(/text-indent:\s*[^;]+;?/gi, '');
            style = style.replace(/text-transform:\s*[^;]+;?/gi, '');
            style = style.replace(/word-spacing:\s*[^;]+;?/gi, '');
            style = style.replace(/-webkit-text-stroke-width:\s*[^;]+;?/gi, '');
            style = style.replace(/white-space:\s*[^;]+;?/gi, '');
            style = style.replace(/orphans:\s*[^;]+;?/gi, '');
            style = style.replace(/widows:\s*[^;]+;?/gi, '');
            style = style.replace(/font-variant[^;]*;?/gi, '');

            // Làm sạch các dấu ; thừa
            style = style.replace(/;+/g, ';').replace(/^;|;$/g, '');

            if (style.trim()) {
                element.attr('style', style);
            } else {
                element.removeAttr('style');
            }
        }
    });

    // 2. Xử lý hình ảnh
    tempDiv.find('img').each(function () {
        var img = $(this);

        // Loại bỏ width cố định, thêm responsive
        img.removeAttr('width').removeAttr('height');
        img.css({
            'max-width': '100%',
            'height': 'auto',
            'display': 'block',
            'margin': '10px auto'
        });
    });

    // 3. Xử lý video containers
    tempDiv.find('.video-container, .tiktok-container').each(function () {
        var container = $(this);
        // Đảm bảo video responsive
        container.css({
            'max-width': '100%',
            'margin': '20px auto'
        });
    });

    // 4. Xử lý bảng
    tempDiv.find('table').each(function () {
        var table = $(this);

        // Wrap table trong div responsive
        if (!table.parent().hasClass('table-responsive')) {
            table.wrap('<div class="table-responsive"></div>');
        }

        table.addClass('table table-bordered');
        table.css({
            'width': '100%',
            'max-width': '100%'
        });
    });

    // 5. Loại bỏ các thuộc tính không cần thiết
    tempDiv.find('*').each(function () {
        var element = $(this);

        // Loại bỏ các thuộc tính
        element.removeAttr('data-sourcepos');
        element.removeAttr('box-sizing');
        element.removeAttr('text-rendering');
        element.removeAttr('font-variant-ligatures');
        element.removeAttr('font-variant-caps');
        element.removeAttr('text-decoration-thickness');
        element.removeAttr('text-decoration-style');
        element.removeAttr('text-decoration-color');

        // Loại bỏ class không cần thiết
        var classAttr = element.attr('class');
        if (classAttr) {
            // Giữ lại các class quan trọng
            var importantClasses = ['table', 'table-responsive', 'video-container', 'tiktok-container'];
            var classes = classAttr.split(' ').filter(cls => importantClasses.includes(cls));

            if (classes.length > 0) {
                element.attr('class', classes.join(' '));
            } else {
                element.removeAttr('class');
            }
        }
    });

    // 6. Loại bỏ các thẻ trống
    tempDiv.find('*').each(function () {
        var element = $(this);
        if (element.is('p, div, span') && element.text().trim() === '' && element.find('img, video, iframe').length === 0) {
            element.remove();
        }
    });

    return tempDiv.html();
}

// Hàm xử lý dữ liệu trước khi hiển thị
function processSummernoteForDisplay(htmlContent) {
    if (!htmlContent) return '';

    var tempDiv = $('<div>').html(htmlContent);

    // Thêm các class Bootstrap và responsive
    tempDiv.find('img').each(function () {
        $(this).addClass('img-fluid');
    });

    tempDiv.find('table').each(function () {
        var table = $(this);
        if (!table.parent().hasClass('table-responsive')) {
            table.wrap('<div class="table-responsive"></div>');
        }
        table.addClass('table table-bordered table-striped');
    });

    // Xử lý video để đảm bảo responsive
    tempDiv.find('.video-container, .tiktok-container').each(function () {
        $(this).css('max-width', '100%');
    });

    return tempDiv.html();
}

// CSS để đảm bảo responsive khi hiển thị
function addResponsiveStyles() {
    if (!$('#responsive-content-styles').length) {
        $('<style id="responsive-content-styles">')
            .html(`
                /* Responsive content styles */
                .content-display {
                    max-width: 100%;
                    overflow-x: auto;
                    word-wrap: break-word;
                }
                
                .content-display img {
                    max-width: 100% !important;
                    height: auto !important;
                    display: block;
                    margin: 10px auto;
                }
                
                .content-display table {
                    width: 100% !important;
                    max-width: 100% !important;
                }
                
                .content-display .table-responsive {
                    overflow-x: auto;
                }
                
                .content-display .video-container,
                .content-display .tiktok-container {
                    max-width: 100% !important;
                    margin: 20px auto !important;
                }
                
                .content-display h1,
                .content-display h2,
                .content-display h3,
                .content-display h4,
                .content-display h5,
                .content-display h6 {
                    word-wrap: break-word;
                    max-width: 100%;
                }
                
                .content-display p,
                .content-display div,
                .content-display span {
                    word-wrap: break-word;
                    max-width: 100%;
                }
                
                @media (max-width: 768px) {
                    .content-display {
                        font-size: 14px;
                    }
                    
                    .content-display h1 { font-size: 1.5em; }
                    .content-display h2 { font-size: 1.3em; }
                    .content-display h3 { font-size: 1.1em; }
                }
            `)
            .appendTo('head');
    }
}


