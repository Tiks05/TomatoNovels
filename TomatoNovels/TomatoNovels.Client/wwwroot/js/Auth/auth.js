window.authWelcome = {
    /**
     * 懒加载视频（纯 JS 控制）：
     * 1. readyState 足够 → 直接淡入；
     * 2. 否则监听 canplaythrough；
     * 3. 再主动调用 play() 兜底自动播放策略。
     */
    initVideoLazyLoad: function (videoId) {
        const video = document.getElementById(videoId);
        if (!video) return;

        const showVideo = () => {
            video.classList.remove("opacity-0");
            video.classList.add("opacity-100");
        };

        // 首屏可能已加载好
        if (video.readyState >= 3) {
            showVideo();
        } else {
            const handler = () => {
                video.removeEventListener("canplaythrough", handler);
                showVideo();
            };
            video.addEventListener("canplaythrough", handler);
        }

        // 自动播放兜底
        try {
            video.muted = true;
            const playPromise = video.play();
            if (playPromise && typeof playPromise.then === "function") {
                playPromise.catch(err => {
                    console.warn("Video autoplay may be blocked:", err);
                });
            }
        } catch (err) {
            console.warn("Video play error:", err);
        }
    }
};
