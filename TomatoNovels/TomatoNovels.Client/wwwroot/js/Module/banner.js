// 确保 Swiper 已经通过 <script src="...swiper-bundle.min.js"></script> 引入

// 一定要挂到 window 上！！！
// 不要写 const initSwiper / function initSwiper，Blazor 找的是 window.initSwiper
window.initSwiper = function () {
    // 等 DOM & Blazor 渲染完成
    setTimeout(function () {
        if (typeof Swiper === "undefined") {
            console.error("Swiper is not loaded, please check script reference.");
            return;
        }

        new Swiper(".mySwiper", {
            loop: true,
            effect: "fade",
            autoplay: {
                delay: 3000,
                disableOnInteraction: false
            },
            pagination: {
                el: ".swiper-pagination",
                clickable: true
            }
        });
    }, 0);
};
