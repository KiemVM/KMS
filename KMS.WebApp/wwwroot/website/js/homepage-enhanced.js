// wwwroot/website/js/homepage-enhanced.js
$(document).ready(function () {
    initializeHomepage();
});

function initializeHomepage() {
    console.log('Initializing homepage...');

    // Khởi tạo carousels
    safeInitializeCarousels();

    // Load PC categories và sản phẩm
    loadPCCategoriesAndProducts();

    // Setup popup
    setupPopup();

    // initNewsPage
    initNewsPage();
}

function initializeCarousels() {
    console.log('Initializing carousels...');

    // Kiểm tra xem Owl Carousel đã load chưa
    if (typeof $.fn.owlCarousel === 'undefined') {
        console.error('Owl Carousel không được load. Kiểm tra thư viện.');
        return;
    }

    // Home slider initialization
    const $homeSlider = $('#js-home-slider');
    if ($homeSlider.length > 0) {
        console.log('Initializing home slider...');

        // Destroy existing carousel nếu có
        if ($homeSlider.hasClass('owl-loaded')) {
            $homeSlider.trigger('destroy.owl.carousel');
            $homeSlider.removeClass('owl-loaded owl-drag');
        }

        // Kiểm tra số lượng slides
        const slideCount = $homeSlider.find('.owl-item, > div, > a, > img').length;
        console.log('Home slider slides count:', slideCount);

        if (slideCount === 0) {
            console.warn('Home slider không có slides nào');
            return;
        }

        //// Initialize home slider
        $homeSlider.owlCarousel({
            items: 1,
            loop: slideCount > 1, // Chỉ loop nếu có nhiều hơn 1 slide
            margin: 0,            // mặc định
            autoplay: slideCount > 1,
            autoplayTimeout: 5000,
            autoplaySpeed: 800,
            autoplayHoverPause: true,
            dots: false,
            lazyLoad: false,
            nav: slideCount > 1,
            navText: [
                "<i class='arrow'></i>",
                "<i class='arrow arrow-right'></i>"
            ],
            responsive: {
                0: {
                    items: 1,
                    margin: 0        // Đổi từ 24 thành 0 cho mobile
                },
                600: {
                    items: 1,
                    margin: 8        // Giảm margin cho tablet
                },
                1024: {
                    items: 1,
                    margin: 16       // Giảm margin cho desktop
                }
            },
            onInitialized: function (event) {
                console.log('Home slider initialized:', event);
                $homeSlider.find('img').each(function () {
                    const $img = $(this);
                    const dataSrc = $img.attr('data-src');
                    if (dataSrc && !$img.attr('src')) {
                        $img.attr('src', dataSrc);
                    }
                    $img.on('error', function () {
                        $(this).attr('src', '/website/images/placeholder-banner.jpg');
                    });
                });
            },
            onChanged: function (event) {
                console.log('Home slider changed to item:', event.item.index);
            }
        });

    } else {
        console.warn('Home slider element (#js-home-slider) không tìm thấy');
    }

    // Collection holders (products) initialization
    initializeProductCarousels();
}

function initializeProductCarousels() {
    const $collectionHolders = $('.js-collection-holder');

    if ($collectionHolders.length > 0) {
        console.log('Initializing product carousels...', $collectionHolders.length);

        $collectionHolders.each(function (index) {
            const $carousel = $(this);
            const carouselId = $carousel.attr('id') || `carousel-${index}`;
            console.log(`Initializing carousel: ${carouselId}`);

            // Destroy existing carousel nếu có
            if ($carousel.hasClass('owl-loaded')) {
                $carousel.trigger('destroy.owl.carousel');
                $carousel.removeClass('owl-loaded owl-drag');
            }

            // Kiểm tra số items
            const itemCount = $carousel.find('.p-item, > div').length;
            console.log(`Carousel ${carouselId} items:`, itemCount);

            if (itemCount === 0) {
                console.warn(`Carousel ${carouselId} không có items`);
                return;
            }

            // Initialize product carousel
            $carousel.owlCarousel({
                margin: 10,
                lazyLoad: false,
                loop: itemCount > 5,
                autoplay: itemCount > 5,
                autoplayTimeout: 4000,
                autoplaySpeed: 1000,
                autoplayHoverPause: true,
                dots: false,
                nav: itemCount > 5,
                navText: [
                    "<i class='arrow'></i>",
                    "<i class='arrow arrow-right'></i>"
                ],
                items: 5,
                responsive: {
                    0: { items: 2, margin: 5 },
                    480: { items: 3, margin: 8 },
                    768: { items: 4, margin: 10 },
                    1024: { items: 5, margin: 10 },
                    1624: { items: 6, margin: 16 }
                },
                onInitialized: function (event) {
                    console.log(`Product carousel ${carouselId} initialized`);

                    // Load images
                    $carousel.find('img').each(function () {
                        const $img = $(this);
                        const dataSrc = $img.attr('data-src');
                        if (dataSrc && !$img.attr('src')) {
                            $img.attr('src', dataSrc);
                        }

                        $img.on('error', function () {
                            $(this).attr('src', '/website/images/no-image.jpg');
                        });
                    });
                }
            });
        });
    } else {
        console.warn('Không tìm thấy product carousels (.js-collection-holder)');
    }
}

function loadPCCategoriesAndProducts() {
    console.log('Loading PC categories and products...');

    // Load sản phẩm cho mỗi PC category container
    $('.js-product-container').each(function () {
        var $container = $(this);
        var categoryId = $container.data('category-id');
        var containerId = $container.find('.p-container').attr('id');

        console.log('Processing container:', containerId, 'Category ID:', categoryId);

        if (categoryId && containerId) {
            loadProductsByCategory(categoryId, '#' + containerId);
        } else {
            console.warn('No category ID or container ID found.');
        }
    });
}

// Function chính để load sản phẩm theo category
function loadProductsByCategory(categoryId, containerId, categoryType = 'auto') {
    console.log('Loading products for category:', categoryId, 'Container:', containerId, 'Type:', categoryType);

    // Hiển thị loading state trước
    showLoadingProducts(containerId);

    // Nếu categoryType là 'auto', thì check category name để xác định loại
    if (categoryType === 'auto') {
        checkCategoryTypeAndLoad(categoryId, containerId);
    } else {
        // Gọi API theo loại đã xác định
        loadItemsByType(categoryId, containerId, categoryType);
    }
}

// Function để check loại category và load tương ứng
function checkCategoryTypeAndLoad(categoryId, containerId) {
    // Gọi API để lấy thông tin category trước
    $.ajax({
        url: `/api/Category/${categoryId}`,
        method: 'GET',
        timeout: 10000,
        success: function (categoryInfo) {
            let categoryType = 'product'; // Mặc định là product

            // Logic để xác định loại category
            if (categoryInfo) {
                // Nếu category có Type = 2 thì là combo
                if (categoryInfo.Type == 2) {
                    categoryType = 'combo';
                }
                // Hoặc check theo tên nếu cần
                else if (categoryInfo.Name && categoryInfo.Name.toLowerCase().includes('combo')) {
                    categoryType = 'combo';
                }
            }

            console.log('Detected category type:', categoryType);
            loadItemsByType(categoryId, containerId, categoryType);
        },
        error: function (xhr, status, error) {
            // Log chi tiết lỗi
            console.error('Error getting category info:', {
                status: status,
                error: error,
                statusCode: xhr.status,
                statusText: xhr.statusText,
                responseText: xhr.responseText,
                categoryId: categoryId
            });

            // Hiển thị thông tin lỗi chi tiết
            let errorMessage = 'Không thể xác định loại category';

            if (xhr.status === 404) {
                errorMessage += ' - Category không tồn tại';
            } else if (xhr.status === 500) {
                errorMessage += ' - Lỗi server';
            } else if (xhr.status === 0) {
                errorMessage += ' - Không thể kết nối tới server';
            } else if (status === 'timeout') {
                errorMessage += ' - Timeout khi gọi API';
            }

            console.log(errorMessage + ', loading both types');

            // Nếu không lấy được thông tin category, mặc định load cả 2 loại
            loadBothProductsAndCombos(categoryId, containerId);
        }
    });
}

// Function để load items theo loại đã xác định
function loadItemsByType(categoryId, containerId, type) {
    const apiEndpoint = type === 'combo'
        ? `/api/Combo/category/${categoryId}`
        : `/api/Product/category/${categoryId}`;

    console.log(`Loading ${type} from:`, apiEndpoint);

    $.ajax({
        url: apiEndpoint,
        method: 'GET',
        data: { count: 10 },
        timeout: 15000,
        success: function (items) {
            console.log(`${type} loaded successfully:`, items);
            if (items && items.length > 0) {
                renderProducts(items, containerId, type);
            } else {
                console.log(`No ${type} found for category:`, categoryId);
                renderNoProducts(containerId);
            }

            // Reinitialize carousel sau khi load xong data
            setTimeout(function () {
                reinitializeCarousel(containerId);
            }, 100);
        },
        error: function (xhr, status, error) {
            // Log chi tiết lỗi
            console.error(`Error loading ${type}:`, {
                status: status,
                error: error,
                statusCode: xhr.status,
                statusText: xhr.statusText,
                responseText: xhr.responseText,
                apiEndpoint: apiEndpoint,
                categoryId: categoryId
            });

            let errorMessage = `Lỗi khi tải ${type}`;

            if (xhr.status === 404) {
                errorMessage += ' - API endpoint không tồn tại';
            } else if (xhr.status === 500) {
                errorMessage += ' - Lỗi server internal';
                try {
                    const response = JSON.parse(xhr.responseText);
                    if (response.message) {
                        errorMessage += ': ' + response.message;
                    }
                } catch (e) {
                    // Ignore JSON parse error
                }
            } else if (xhr.status === 0) {
                errorMessage += ' - Không thể kết nối tới server';
            } else if (status === 'timeout') {
                errorMessage += ' - Timeout khi gọi API';
            }

            console.error(errorMessage);
            if (xhr.status != 404)
                renderErrorProducts(containerId, errorMessage);
            else {
                renderNoProductProducts(containerId);
            }
            // Reinitialize carousel even on error
            setTimeout(function () {
                reinitializeCarousel(containerId);
            }, 100);
        }
    });
}

// Function để load cả product và combo, sau đó merge kết quả
function loadBothProductsAndCombos(categoryId, containerId) {
    console.log('Loading both products and combos for category:', categoryId);

    let productData = [];
    let comboData = [];
    let completedRequests = 0;
    let errors = [];

    // Function để xử lý khi cả 2 request hoàn thành
    function processResults() {
        completedRequests++;
        console.log(`Completed requests: ${completedRequests}/2`);

        if (completedRequests === 2) {
            console.log('Product data:', productData);
            console.log('Combo data:', comboData);
            console.log('Errors:', errors);

            // Merge và sort theo SoldCount
            const allItems = [...productData, ...comboData]
                .sort((a, b) => (b.soldCount || 0) - (a.soldCount || 0))
                .slice(0, 10); // Lấy top 10

            console.log('Merged items:', allItems);

            if (allItems.length > 0) {
                renderProducts(allItems, containerId, 'mixed');
            } else {
                // Nếu không có items nào, hiển thị lỗi nếu có
                if (errors.length > 0) {
                    renderErrorProducts(containerId, 'Lỗi: ' + errors.join(', '));
                } else {
                    renderNoProducts(containerId);
                }
            }

            setTimeout(function () {
                reinitializeCarousel(containerId);
            }, 100);
        }
    }

    // Load products
    $.ajax({
        url: `/api/Product/category/${categoryId}`,
        method: 'GET',
        data: { count: 5 },
        timeout: 15000,
        success: function (products) {
            console.log('Products loaded successfully:', products);
            productData = products || [];
        },
        error: function (xhr, status, error) {
            console.error('Error loading products:', {
                status: status,
                error: error,
                statusCode: xhr.status,
                statusText: xhr.statusText,
                responseText: xhr.responseText
            });

            let errorMsg = `Products API error: ${xhr.status} ${xhr.statusText}`;
            if (status === 'timeout') errorMsg = 'Products API timeout';
            errors.push(errorMsg);
            productData = [];
        },
        complete: function () {
            console.log('Products request completed');
            processResults();
        }
    });

    // Load combos
    $.ajax({
        url: `/api/Combo/category/${categoryId}`,
        method: 'GET',
        data: { count: 5 },
        timeout: 15000,
        success: function (combos) {
            console.log('Combos loaded successfully:', combos);
            // Thêm flag để phân biệt combo với product trong rendering
            comboData = (combos || []).map(combo => ({
                ...combo,
                isCombo: true
            }));
        },
        error: function (xhr, status, error) {
            console.error('Error loading combos:', {
                status: status,
                error: error,
                statusCode: xhr.status,
                statusText: xhr.statusText,
                responseText: xhr.responseText
            });

            let errorMsg = `Combos API error: ${xhr.status} ${xhr.statusText}`;
            if (status === 'timeout') errorMsg = 'Combos API timeout';
            errors.push(errorMsg);
            comboData = [];
        },
        complete: function () {
            console.log('Combos request completed');
            processResults();
        }
    });
}

// Function để render products với support cho cả product và combo
function renderProducts(items, containerId, type = 'product') {
    let html = '<div class="owl-carousel owl-theme custom-nav">';

    items.forEach(function (item) {
        // Xử lý data format khác nhau giữa API responses
        const itemData = {
            id: item.id || item.Id,
            name: item.name || item.Name,
            image: item.image || item.Image,
            slug: item.slug || item.Slug,
            priceSale: item.priceSale || item.PriceSale,
            priceListed: item.priceListed || item.PriceListed,
            stock: item.stock || item.Stock,
            soldCount: item.soldCount || item.SoldCount,
            specifications: item.specifications || item.Specifications,
            isCombo: item.isCombo || type === 'combo'
        };

        const imageUrl = itemData.image || '/website/images/no-image.jpg';
        const itemUrl = itemData.isCombo
            ? `/combo-pc/${itemData.slug || itemData.id}`
            : `/san-pham-pc/${itemData.slug || itemData.id}`;
        const price = formatCurrency(itemData.priceSale);
        const oldPrice = itemData.priceListed > itemData.priceSale ? formatCurrency(itemData.priceListed) : '';
        const discount = itemData.priceListed > itemData.priceSale ?
            Math.round(((itemData.priceListed - itemData.priceSale) / itemData.priceListed) * 100) : 0;
        const stockStatus = itemData.stock > 0 ?
            '<span style="color: #0DB866;"><i class="icons icon-check"></i> Còn hàng</span>' :
            '<span style="color: #ff6b6b;"><i class="icons icon-close"></i> Hết hàng</span>';

        // Badge để phân biệt product vs combo
        const typeBadge = itemData.isCombo
            ? '<span class="item-type-badge combo-badge">COMBO</span>'
            : '<span class="item-type-badge product-badge">SẢN PHẨM</span>';

        // Xử lý specifications
        let specificationsHtml = '';
        if (itemData.specifications) {
            const specs = itemData.specifications.split('\n').slice(0, 3);
            specificationsHtml = specs.map(spec =>
                spec.trim() ? `<div class="item"><p>${spec.trim()}</p></div>` : ''
            ).join('');
        }

        html += `
            <div class="p-item">
                <a href="${itemUrl}" class="p-img">
                    <img src="${imageUrl}" alt="${itemData.name}" width="250" height="250" 
                         onerror="this.src='/website/images/no-image.jpg'">
                </a>
                <div class="p-text">
                    <a href="${itemUrl}" class="p-name">
                        <h3 class="inherit">${itemData.name}</h3>
                    </a>
                    <div class="p-price-group">
                        <span class="p-price">${price}</span>
                        ${oldPrice ? `<del class="p-old-price">${oldPrice}</del>` : ''}
                        ${discount > 0 ? `<span class="p-discount">(Tiết kiệm: ${discount}%)</span>` : ''}
                    </div>
                    <div class="p-btn-group">
                        <p>${stockStatus}</p>
                        <a href="javascript:void(0)" class="p-add-cart" onclick="addProductToCart('${itemData.id}', 1, '')"></a>
                    </div>
                </div>
                <div class="p-tooltip">
                    <p class="tooltip-title">${itemData.name}</p>
                    <div class="tooltip-content">
                        <table>
                            <tbody>
                                <tr>
                                    <td>Loại</td>
                                    <td>${itemData.isCombo ? 'Combo sản phẩm' : 'Sản phẩm đơn'}</td>
                                </tr>
                                <tr>
                                    <td>Giá bán</td>
                                    <td><span class="tooltip-price">${price}</span></td>
                                </tr>
                                ${itemData.soldCount > 0 ? `<tr><td>Đã bán</td><td>${itemData.soldCount} ${itemData.isCombo ? 'combo' : 'sản phẩm'}</td></tr>` : ''}
                                <tr><td>Bảo hành</td><td>theo từng linh kiện</td></tr>
                            </tbody>
                        </table>
                        ${specificationsHtml ? `
                            <div class="tooltip-content-item">
                                <b class="title"><i class="tooltip-icon icon-doc"></i> Thông số sản phẩm</b>
                                <div class="tooltip-content-list">
                                    ${specificationsHtml}
                                </div>
                            </div>
                        ` : ''}
                    </div>
                </div>
            </div>
        `;
    });

    html += '</div>';
    $(containerId).html(html);
}

// Utility functions
function renderNoProducts(containerId) {
    let html = '<div class="owl-carousel owl-theme custom-nav">';
    for (let i = 0; i < 5; i++) {
        html += `
            <div class="p-item">
                <div class="p-img">
                    <img src="/website/images/no-image.jpg" alt="Không có sản phẩm" width="250" height="250">
                </div>
                <div class="p-text">
                    <div class="p-name">
                        <h3 class="inherit">Đang cập nhật sản phẩm...</h3>
                    </div>
                    <div class="p-price-group">
                        <span class="p-price">0 đ</span>
                    </div>
                </div>
            </div>
        `;
    }
    html += '</div>';
    $(containerId).html(html);
}

function renderErrorProducts(containerId, errorMessage = 'Lỗi tải sản phẩm') {
    console.error('Rendering error products:', errorMessage);

    let html = '<div class="owl-carousel owl-theme custom-nav">';
    for (let i = 0; i < 3; i++) {
        html += `
            <div class="p-item error-item">
                <div class="p-img">
                    <div class="error-icon" style="
                        width: 250px; 
                        height: 250px; 
                        display: flex; 
                        align-items: center; 
                        justify-content: center; 
                        background: #f8f9fa; 
                        border: 2px dashed #dc3545;
                        color: #dc3545;
                        font-size: 48px;
                    ">⚠️</div>
                </div>
                <div class="p-text">
                    <div class="p-name">
                        <h3 class="inherit" style="color: #dc3545;">${errorMessage}</h3>
                    </div>
                    <div class="p-price-group">
                        <span class="p-price" style="color: #6c757d;">Vui lòng thử lại</span>
                    </div>
                    <div class="p-btn-group">
                        <button onclick="location.reload()" class="btn btn-sm btn-outline-primary">
                            Tải lại trang
                        </button>
                    </div>
                </div>
            </div>
        `;
    }
    html += '</div>';
    $(containerId).html(html);

    console.log('Error products rendered in:', containerId);
}

function renderNoProductProducts(containerId) {
    let html = '<div class="owl-carousel owl-theme custom-nav">';
    for (let i = 0; i < 1; i++) {
        html += `
            <h3>Không có sản phẩm nào trong danh mục này</h3>
        `;
    }
    html += '</div>';
    $(containerId).html(html);
}

function showLoadingProducts(containerId) {
    let html = '<div class="owl-carousel owl-theme custom-nav">';
    for (let i = 0; i < 6; i++) {
        html += `
            <div class="p-item loading-placeholder">
                <div class="p-img">
                    <div class="skeleton-loader" style="width: 250px; height: 250px;"></div>
                </div>
                <div class="p-text">
                    <div class="p-name">
                        <div class="skeleton-loader" style="width: 100%; height: 40px; margin-bottom: 10px;"></div>
                    </div>
                    <div class="p-price-group">
                        <div class="skeleton-loader" style="width: 120px; height: 20px;"></div>
                    </div>
                </div>
            </div>
        `;
    }
    html += '</div>';
    $(containerId).html(html);
}

// Function để reinitialize carousel sau khi load data
function reinitializeCarousel(containerId) {
    console.log('Reinitializing carousel:', containerId);

    const $carousel = $(containerId + ' .owl-carousel');

    if ($carousel.length === 0) {
        console.warn('Carousel not found for reinitialize:', containerId);
        return;
    }

    // Destroy existing carousel
    if ($carousel.hasClass('owl-loaded')) {
        $carousel.trigger('destroy.owl.carousel');
        $carousel.removeClass('owl-loaded owl-drag');
        console.log('Destroyed existing carousel');
    }

    // Wait a bit for DOM to settle
    setTimeout(function () {
        const itemCount = $carousel.find('.p-item').length;
        console.log(`Reinitializing carousel with ${itemCount} items`);

        if (itemCount === 0) {
            console.warn('No items to reinitialize carousel');
            return;
        }

        // Reinitialize
        $carousel.owlCarousel({
            margin: 10,
            lazyLoad: false,
            loop: itemCount > 5,
            autoplay: itemCount > 5,
            autoplayTimeout: 4000,
            autoplaySpeed: 1000,
            autoplayHoverPause: true,
            dots: false,
            nav: itemCount > 5,
            navText: [
                "<i class='arrow'></i>",
                "<i class='arrow arrow-right'></i>"
            ],
            items: 5,
            responsive: {
                0: { items: 2, margin: 5 },
                480: { items: 3, margin: 8 },
                768: { items: 4, margin: 10 },
                1024: { items: 5, margin: 10 },
                1624: { items: 6, margin: 16 }
            },
            onInitialized: function () {
                console.log('Carousel reinitialized successfully');

                // Force load images
                $carousel.find('img').each(function () {
                    const $img = $(this);
                    if (!$img.attr('src') && $img.attr('data-src')) {
                        $img.attr('src', $img.attr('data-src'));
                    }

                    $img.on('error', function () {
                        $(this).attr('src', '/website/images/no-image.jpg');
                    });
                });
            }
        });
    }, 100);
}

// Enhanced initialization với error handling
function safeInitializeCarousels() {
    try {
        // Wait for DOM and images to be ready
        if (document.readyState === 'loading') {
            $(document).ready(function () {
                setTimeout(initializeCarousels, 500);
            });
        } else {
            setTimeout(initializeCarousels, 500);
        }
    } catch (error) {
        console.error('Error initializing carousels:', error);
    }
}

// Utility functions
function formatCurrency(amount) {
    if (!amount) return '0 đ';
    return new Intl.NumberFormat('vi-VN').format(amount) + ' đ';
}

function setupPopup() {
    setTimeout(function () {
        showPopup();
    }, 3000);
}

function showPopup() {
    $('.popup-container').fadeIn();
    sessionStorage.setItem('popupShown', 'true');
}

function closePopup() {
    $('.popup-container').fadeOut();
}

function showNotification(message, type = 'success') {
    console.log(`${type.toUpperCase()}: ${message}`);
}

// Event handlers
$(document).on('click', '.bg-popup', function (e) {
    if (e.target === this) {
        closePopup();
    }
});

$(document).keydown(function (e) {
    if (e.keyCode === 27) { // ESC key
        closePopup();
    }
});

// Export functions to global scope
window.initializeCarousels = initializeCarousels;
window.reinitializeCarousel = reinitializeCarousel;
window.safeInitializeCarousels = safeInitializeCarousels;
window.loadProductsByCategory = loadProductsByCategory;

// init các dữ liệu cho NewsPage
function initNewsPage() {
    console.log('Initializing News page ...')
    initCarouselForNews();
    loadNewsByCategory();
}

function initCarouselForNews() {

}

function loadNewsByCategory() {
    console.log('Loading news by category ...')
    // Load sản phẩm cho mỗi PC category container
    $('.js-news-container').each(function () {
        var $container = $(this);
        var categoryId = $container.data('category-id');
        var containerId = $container.find('.p-container-news').attr('id');

        console.log('Processing container:', containerId, 'Category ID:', categoryId);

        if (categoryId && containerId) {
            loadNewsByCategoryId(categoryId, '#' + containerId);
        } else {
            // Nếu không có categoryId, hiển thị placeholder
            console.warn('No category ID or container ID found, skipping news load.');
        }
    });
}

function loadNewsByCategoryId(categoryId, containerId) {
    console.log('Loading news for category:', categoryId);
    // Hiển thị loading state trước
    //showLoadingProducts(containerId);

    const apiEndpoint = `/api/Article/category/${categoryId}`;

    $.ajax({
        url: apiEndpoint,
        method: 'GET',
        data: { count: 10 },
        timeout: 15000,
        success: function (items) {
            console.log(`loaded successfully:`, items);
            if (items && items.length > 0) {
                renderArticles(items, containerId);
            } else {
                console.log(`No ${type} found for category:`, categoryId);
            }

            // Reinitialize carousel sau khi load xong data
            setTimeout(function () {
                reinitializeCarousel(containerId);
            }, 100);
        },
        error: function (xhr, status, error) {
            // Log chi tiết lỗi
            console.error(`Error loading:`, {
                status: status,
                error: error,
                statusCode: xhr.status,
                statusText: xhr.statusText,
                responseText: xhr.responseText,
                apiEndpoint: apiEndpoint,
                categoryId: categoryId
            });
           
            // Reinitialize carousel even on error
            setTimeout(function () {
                reinitializeCarousel(containerId);
            }, 100);
        }
    });
}

function renderArticles(items, containerId) {
    items.forEach(function (item) {
        // Xử lý data format khác nhau giữa API responses
        const itemData = {
            id: item.Id,
            title: item.Name,
            image: item.SeoImages,
            slug: item.Slug,
            publishedDate: item.DateCreated,
            shortDescription: item.Description
        };
        const imageUrl = itemData.image || '/website/images/no-image.jpg';
        const itemUrl = `/tin-tuc-chi-tiet/${itemData.slug}`;
        const publishedDate = new Date(itemData.publishedDate).toLocaleDateString('vi-VN');
        let html = `
            <div class="art-item col-12 col-md-4">
                <a href="${itemUrl}" class="art-img">
                    <img src="${imageUrl}"
                            alt="${item.Name}"
                            width="1" height="1" class="lazy"/>
                </a>
                <div class="art-text">
                    <p class="art-time">
                        by <b>${item.AvatarViewModel?.UserName}</b> |
                        <time>
                            ${publishedDate}
                        </time>
                    </p>
                    <a href="${itemUrl}" class="art-title">
                        <h3 class="inherit">${item.Name}</h3>
                    </a>

                </div>
            </div>
        `;
        $(containerId).append(html);
    });
}