// Hàm cập nhật số lượng sản phẩm trong giỏ
async function updateCartQuantity(productId, quantity) {
    try {
        console.log(`Updating quantity: ${productId} -> ${quantity}`);

        // Đảm bảo session tồn tại
        await ensureSession();

        const response = await fetch('/api/Cart/UpdateQuantity', {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                productId: productId.toString(),
                quantity: parseInt(quantity)
            })
        });

        const result = await response.json();

        if (result.Success) {
            // Cập nhật cart count
            await updateCartCount();

            // Nếu đang ở trang giỏ hàng, cập nhật UI
            if (window.location.pathname.includes('/gio-hang')) {
                // Cập nhật tổng tiền của item
                updateCartItemTotal(productId, result.CartSummary);
            }

            return result;
        } else {
            showCartMessage(result.Message || 'Có lỗi xảy ra khi cập nhật số lượng', 'error');
            return null;
        }
    } catch (error) {
        showCartMessage('Có lỗi xảy ra khi cập nhật số lượng', 'error');
        console.error('Error updating quantity:', error);
        return null;
    }
}

// Hàm xóa sản phẩm khỏi giỏ hàng
async function removeFromCart(productId) {
    if (!confirm('Bạn có chắc chắn muốn xóa sản phẩm này khỏi giỏ hàng?')) {
        return;
    }

    try {
        console.log(`Removing product from cart: ${productId}`);

        // Đảm bảo session tồn tại
        await ensureSession();

        const response = await fetch(`/api/Cart/RemoveItem/${productId}`, {
            method: 'DELETE',
            credentials: 'include'
        });

        const result = await response.json();

        if (result.Success) {
            showCartMessage('Đã xóa sản phẩm khỏi giỏ hàng', 'success');

            // Cập nhật cart count
            await updateCartCount();

            // Nếu đang ở trang giỏ hàng, xóa dòng sản phẩm
            const productRow = document.querySelector(`[data-product-id="${productId}"]`);
            if (productRow) {
                // Animation fade out
                productRow.style.opacity = '0.5';
                productRow.style.transform = 'translateX(-20px)';
                productRow.style.transition = 'all 0.3s ease';

                setTimeout(() => {
                    productRow.remove();

                    // Kiểm tra nếu giỏ hàng trống
                    const remainingItems = document.querySelectorAll('.cart-item');
                    if (remainingItems.length === 0) {
                        showEmptyCartMessage();
                    } else {
                        // Cập nhật tổng tiền
                        updateCartSummary(result.CartSummary);
                    }
                }, 300);
            }

            return result;
        } else {
            showCartMessage(result.Message || 'Có lỗi xảy ra khi xóa sản phẩm', 'error');
        }
    } catch (error) {
        showCartMessage('Có lỗi xảy ra khi xóa sản phẩm', 'error');
        console.error('Error removing from cart:', error);
    }
}

// Hàm lấy thông tin giỏ hàng
async function getCartInfo() {
    try {
        await ensureSession();

        const response = await fetch('/api/Cart/GetCart', {
            credentials: 'include'
        });

        const cart = await response.json();
        return cart;
    } catch (error) {
        console.error('Error getting cart info:', error);
        return null;
    }
}

// Hàm xóa toàn bộ giỏ hàng
async function clearCart() {
    if (!confirm('Bạn có chắc chắn muốn xóa toàn bộ giỏ hàng?')) {
        return;
    }

    try {
        await ensureSession();

        const response = await fetch('/api/Cart/ClearCart', {
            method: 'POST',
            credentials: 'include'
        });

        const result = await response.json();

        if (result.Success) {
            showCartMessage('Đã xóa toàn bộ giỏ hàng', 'success');

            // Cập nhật cart count
            updateCartCount();

            // Nếu đang ở trang giỏ hàng, reload trang
            if (window.location.pathname.includes('/gio-hang')) {
                setTimeout(() => {
                    location.reload();
                }, 1500);
            }

            return result;
        } else {
            showCartMessage(result.Message || 'Có lỗi xảy ra khi xóa giỏ hàng', 'error');
        }
    } catch (error) {
        showCartMessage('Có lỗi xảy ra khi xóa giỏ hàng', 'error');
        console.error('Error clearing cart:', error);
    }
}

// Helper functions cho trang giỏ hàng

// Cập nhật tổng tiền của từng item
function updateCartItemTotal(productId, cartSummary) {
    const itemRow = document.querySelector(`[data-product-id="${productId}"]`);
    if (itemRow && cartSummary) {
        const item = cartSummary.items?.find(i => i.productId === productId);
        if (item) {
            const totalElement = itemRow.querySelector('.item-total');
            if (totalElement) {
                totalElement.textContent = item.totalPrice.toLocaleString('vi-VN') + ' đ';
            }
        }

        // Cập nhật cart summary
        updateCartSummary(cartSummary);
    }
}

// Cập nhật tổng kết giỏ hàng
function updateCartSummary(cartSummary) {
    if (!cartSummary) return;

    // Cập nhật tạm tính
    const subtotalElement = document.querySelector('.summary-row:not(.discount):not(.total) span:last-child');
    if (subtotalElement) {
        const subtotal = cartSummary.TotalAmount + (cartSummary.TotalDiscount || 0);
        subtotalElement.textContent = subtotal.toLocaleString('vi-VN') + ' đ';
    }

    // Cập nhật giảm giá
    const discountElement = document.querySelector('.summary-row.discount span:last-child');
    if (discountElement && cartSummary.TotalDiscount > 0) {
        discountElement.textContent = '-' + cartSummary.TotalDiscount.toLocaleString('vi-VN') + ' đ';
    }

    // Cập nhật tổng cộng
    const totalElement = document.querySelector('.summary-row.total span:last-child');
    if (totalElement) {
        totalElement.textContent = cartSummary.TotalAmount.toLocaleString('vi-VN') + ' đ';
    }

    // Cập nhật số lượng items
    const countInfo = document.querySelector('.cart-count-info span');
    if (countInfo) {
        if (cartSummary.TotalItems > 0) {
            countInfo.innerHTML = `Có <strong>${cartSummary.TotalItems}</strong> sản phẩm trong giỏ hàng`;
        } else {
            countInfo.textContent = 'Giỏ hàng của bạn đang trống';
        }
    }
}

// Hiển thị thông báo giỏ hàng trống
function showEmptyCartMessage() {
    const cartContent = document.querySelector('.cart-content');
    if (cartContent) {
        cartContent.innerHTML = `
            <div class="empty-cart">
                <div class="empty-cart-icon">
                    <i class="fas fa-shopping-cart"></i>
                </div>
                <h2>Giỏ hàng của bạn đang trống</h2>
                <p>Hãy thêm sản phẩm vào giỏ hàng để tiếp tục mua sắm</p>
                <a href="/" class="btn-start-shopping">Bắt đầu mua sắm</a>
            </div>
        `;
    }
}

// JavaScript cho cross-page session persistence

// Hàm khởi tạo session trước khi làm gì khác
async function ensureSession() {
    try {
        console.log('Ensuring session exists...');

        // Gọi API để đảm bảo session được tạo
        const response = await fetch('/api/Cart/TestSession', {
            method: 'GET',
            credentials: 'include',
            headers: {
                'Cache-Control': 'no-cache'
            }
        });

        if (!response.ok) {
            throw new Error(`Session init failed: ${response.status}`);
        }

        const data = await response.json();
        console.log('Session ensured:', data.sessionId);

        // Lưu vào localStorage để track
        localStorage.setItem('currentSessionId', data.sessionId);
        localStorage.setItem('sessionInitTime', Date.now().toString());

        return data.sessionId;
    } catch (error) {
        console.error('Error ensuring session:', error);
        return null;
    }
}

// Hàm kiểm tra session có thay đổi không
function checkSessionConsistency() {
    const lastSessionId = localStorage.getItem('currentSessionId');
    const initTime = localStorage.getItem('sessionInitTime');

    console.log('Last known session:', lastSessionId);
    console.log('Session init time:', new Date(parseInt(initTime || '0')));

    return { lastSessionId, initTime };
}

// Hàm cập nhật cart count với session consistency check
async function updateCartCount() {
    try {
        // Kiểm tra session trước
        const { lastSessionId } = checkSessionConsistency();

        console.log('Fetching cart count...');

        const response = await fetch('/api/Cart/GetCartCount', {
            method: 'GET',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json',
                'Cache-Control': 'no-cache'
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP ${response.status}`);
        }

        const responseData = await response.json();
        console.log('Cart count response:', responseData);

        // Kiểm tra session consistency
        if (lastSessionId && responseData.sessionId && lastSessionId !== responseData.sessionId) {
            console.warn(`Session changed! Old: ${lastSessionId}, New: ${responseData.sessionId}`);
            // Update stored session
            localStorage.setItem('currentSessionId', responseData.sessionId);
        }

        const count = responseData || 0;
        updateCartUI(count);

        return count;

    } catch (error) {
        console.error('Error updating cart count:', error);

        // Fallback: thử khởi tạo session mới
        console.log('Attempting to reinitialize session...');
        const newSessionId = await ensureSession();
        if (newSessionId) {
            // Thử lại
            return updateCartCount();
        } else {
            updateCartUI(0);
            return 0;
        }
    }
}

// Hàm thêm sản phẩm với session check
async function addProductToCart(productId, quantity = 1, redirectUrl = '') {
    try {
        // Đảm bảo session tồn tại trước
        await ensureSession();

        showCartLoading(true);

        const requestData = {
            productId: productId.toString(),
            quantity: parseInt(quantity) || 1,
            redirectUrl: redirectUrl || ''
        };

        console.log('Adding to cart:', requestData);

        const response = await fetch('/api/Cart/AddToCart', {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(requestData)
        });

        const result = await response.json();

        if (result.Success) {
            showCartMessage('Đã thêm sản phẩm vào giỏ hàng!', 'success');

            // Đợi một chút để session được persist
            setTimeout(async () => {
                await updateCartCount();

                if (redirectUrl && redirectUrl !== '') {
                    // Đợi thêm để đảm bảo session được save
                    setTimeout(() => {
                        window.location.href = redirectUrl;
                    }, 500);
                }
            }, 300);

        } else {
            showCartMessage(result.Message || 'Có lỗi xảy ra khi thêm sản phẩm', 'error');
        }
    } catch (error) {
        showCartMessage('Có lỗi xảy ra khi thêm sản phẩm vào giỏ hàng', 'error');
        console.error('Error adding to cart:', error);
    } finally {
        showCartLoading(false);
    }
}

// Hàm test session xuyên trang
async function testCrossPageSession() {
    console.log('=== CROSS PAGE SESSION TEST ===');

    // Test 1: Khởi tạo session
    const sessionId1 = await ensureSession();
    console.log('1. Session initialized:', sessionId1);

    // Test 2: Thêm sản phẩm
    // (simulate adding product)
    console.log('2. Simulating add to cart...');

    // Test 3: Kiểm tra lại sau khi add
    setTimeout(async () => {
        const count = await updateCartCount();
        console.log('3. Cart count after add:', count);

        // Test 4: Lưu thông tin để check sau khi chuyển trang
        localStorage.setItem('crossPageTest', JSON.stringify({
            sessionId: sessionId1,
            timestamp: Date.now(),
            cartCount: count
        }));

        console.log('=== TEST COMPLETED - Check after page navigation ===');
    }, 1000);
}

// Kiểm tra khi trang load xem session có consistent không
function checkSessionAfterPageLoad() {
    const testData = localStorage.getItem('crossPageTest');
    if (testData) {
        try {
            const parsed = JSON.parse(testData);
            console.log('Previous page session data:', parsed);

            // So sánh với session hiện tại
            setTimeout(async () => {
                const currentData = await fetch('/api/Cart/TestSession', { credentials: 'include' })
                    .then(r => r.json());

                console.log('Current page session:', currentData.sessionId);
                console.log('Session consistent:', parsed.sessionId === currentData.sessionId);

                if (parsed.sessionId !== currentData.sessionId) {
                    console.error('❌ SESSION CHANGED BETWEEN PAGES!');
                } else {
                    console.log('✅ Session is consistent across pages');
                }

                // Cleanup
                localStorage.removeItem('crossPageTest');
            }, 500);

        } catch (error) {
            console.error('Error parsing test data:', error);
        }
    }
}

function updateCartUI(count) {
    const cartBadges = document.querySelectorAll('.cart-count, .cart-badge, #header-cart-count');
    cartBadges.forEach(badge => {
        if (badge) {
            badge.textContent = count;
            //badge.style.display = count > 0 ? 'inline-flex' : 'none';
        }
    });

    const cartTexts = document.querySelectorAll('.cart-count-text');
    cartTexts.forEach(text => {
        if (text) {
            text.textContent = count > 0 ? `${count} sản phẩm` : 'Giỏ hàng trống';
        }
    });
}

function showCartLoading(show) {
    const buttons = document.querySelectorAll('.pd-btn-add-product, .pd-btn-buyNow');
    buttons.forEach(button => {
        if (show) {
            button.style.opacity = '0.6';
            button.style.pointerEvents = 'none';
            const originalText = button.innerHTML;
            button.setAttribute('data-original-text', originalText);
            button.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Đang thêm...';
        } else {
            button.style.opacity = '1';
            button.style.pointerEvents = 'auto';
            const originalText = button.getAttribute('data-original-text');
            if (originalText) {
                button.innerHTML = originalText;
            }
        }
    });
}

function showCartMessage(message, type = 'info') {
    let messageDiv = document.getElementById('cart-message');
    if (!messageDiv) {
        messageDiv = document.createElement('div');
        messageDiv.id = 'cart-message';
        messageDiv.style.cssText = `
            position: fixed; top: 20px; right: 20px; padding: 15px 20px;
            border-radius: 5px; z-index: 9999; font-weight: bold; min-width: 300px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.15); transition: all 0.3s ease;
        `;
        document.body.appendChild(messageDiv);
    }

    const colors = {
        success: { bg: '#d4edda', color: '#155724', border: '#c3e6cb' },
        error: { bg: '#f8d7da', color: '#721c24', border: '#f5c6cb' },
        info: { bg: '#d1ecf1', color: '#0c5460', border: '#bee5eb' }
    };

    const colorScheme = colors[type] || colors.info;
    messageDiv.style.backgroundColor = colorScheme.bg;
    messageDiv.style.color = colorScheme.color;
    messageDiv.style.border = `1px solid ${colorScheme.border}`;
    messageDiv.textContent = message;
    messageDiv.style.display = 'block';
    messageDiv.style.opacity = '1';

    setTimeout(() => {
        messageDiv.style.opacity = '0';
        setTimeout(() => {
            if (messageDiv.parentNode) {
                messageDiv.parentNode.removeChild(messageDiv);
            }
        }, 300);
    }, 3000);
}

// Khởi tạo khi trang load
document.addEventListener('DOMContentLoaded', async function () {
    console.log('Page loaded - initializing cart system...');

    //// Kiểm tra session consistency từ trang trước
    //checkSessionAfterPageLoad();

    //// Đảm bảo session tồn tại
    //await ensureSession();

    //// Load cart count
    //await updateCartCount();

    //// Debug mode
    //if (window.location.search.includes('debug=true')) {
    //    setTimeout(() => testCrossPageSession(), 2000);
    //}

    console.log('Cart system initialized');
});

// Export functions
window.CartFunctions = {
    addProductToCart,
    updateCartCount,
    ensureSession,
    testCrossPageSession,
    checkSessionAfterPageLoad
};