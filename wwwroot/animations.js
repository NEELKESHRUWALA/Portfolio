// ── Animated Favicon ─────────────────────────────────────────────
(function animateFavicon() {
    const canvas = document.createElement('canvas');
    canvas.width = 64;
    canvas.height = 64;
    const ctx = canvas.getContext('2d');

    // Find or create the favicon link element
    let link = document.querySelector("link[rel~='icon']");
    if (!link) {
        link = document.createElement('link');
        link.rel = 'icon';
        document.head.appendChild(link);
    }

    let angle = 0;

    function draw() {
        ctx.clearRect(0, 0, 64, 64);

        // Background circle
        ctx.beginPath();
        ctx.arc(32, 32, 30, 0, Math.PI * 2);
        ctx.fillStyle = '#0a0a0a';
        ctx.fill();

        // Rotating gold arc
        ctx.beginPath();
        ctx.arc(32, 32, 27, angle, angle + Math.PI * 1.4);
        ctx.strokeStyle = '#c9a96e';
        ctx.lineWidth = 2.5;
        ctx.lineCap = 'round';
        ctx.stroke();

        // Second trailing arc (dimmer)
        ctx.beginPath();
        ctx.arc(32, 32, 27, angle + Math.PI * 1.5, angle + Math.PI * 1.9);
        ctx.strokeStyle = 'rgba(201,169,110,0.3)';
        ctx.lineWidth = 1.5;
        ctx.stroke();

        // "NK" initials in center
        ctx.font = 'bold 18px serif';
        ctx.fillStyle = '#f0ede8';
        ctx.textAlign = 'center';
        ctx.textBaseline = 'middle';
        ctx.fillText('NK', 32, 33);

        // Update favicon
        link.href = canvas.toDataURL('image/png');

        angle += 0.04;
        requestAnimationFrame(draw);
    }

    draw();
})();

// ── Dual-ring cursor ──────────────────────────────────────────────
const dot = document.getElementById('cursor-dot');
const ring = document.getElementById('cursor-ring');

if (dot && ring) {
    let mx = 0, my = 0, rx = 0, ry = 0;
    window.addEventListener('mousemove', e => { mx = e.clientX; my = e.clientY; });

    let ticking = false;
    function tick() {
        if (dot) { dot.style.left = mx + 'px'; dot.style.top = my + 'px'; }
        if (ring) {
            rx += (mx - rx) * 0.12;
            ry += (my - ry) * 0.12;
            ring.style.left = rx + 'px';
            ring.style.top = ry + 'px';
        }
        ticking = false;
    }

    window.addEventListener('mousemove', () => {
        if (!ticking) {
            requestAnimationFrame(tick);
            ticking = true;
        }
    });

    // Magnetic effect on interactive elements
    const interactives = document.querySelectorAll('button, a, .project-row, .nav-link, .skill-item, .edu-card, .modal-close, input, textarea');
    interactives.forEach(el => {
        el.style.cursor = 'none';
        el.addEventListener('mouseenter', () => ring?.classList.add('magnetic'));
        el.addEventListener('mouseleave', () => ring?.classList.remove('magnetic'));
    });

    // Click effect
    window.addEventListener('mousedown', () => {
        if (dot) dot.style.transform = 'translate(-50%, -50%) scale(0.6)';
    });
    window.addEventListener('mouseup', () => {
        if (dot) dot.style.transform = 'translate(-50%, -50%) scale(1)';
    });
}

// ── Scroll Float (Parallax) ──────────────────────────────────────
window.addEventListener('scroll', () => {
    const scrolled = window.pageYOffset;

    // Float the Hero Image slightly faster than scroll
    const heroImg = document.querySelector('.hero-image-frame');
    if (heroImg) {
        heroImg.style.transform = `translateY(${scrolled * 0.15}px)`;
    }

    // Drift the Section Labels
    document.querySelectorAll('.section-label').forEach(label => {
        const speed = 0.05;
        label.style.transform = `translateX(${scrolled * speed}px)`;
    });

    // Parallax for Background Accents
    const accents = document.querySelectorAll('.image-accent');
    accents.forEach(acc => {
        acc.style.transform = `translateY(${scrolled * -0.1}px) rotate(${scrolled * 0.02}deg)`;
    });
});

// ── Holographic Tilt Engine ──────────────────────────────────────
function initHoloTilt() {
    const card = document.getElementById('hero-holo-card');
    if (!card) return;

    let targetX = 0, targetY = 0;
    let currentX = 0, currentY = 0;
    const lerp = 0.1; // Smoothness

    card.addEventListener('pointermove', (e) => {
        const rect = card.getBoundingClientRect();
        const x = e.clientX - rect.left;
        const y = e.clientY - rect.top;

        const px = (x / rect.width) * 100;
        const py = (y / rect.height) * 100;

        targetX = px;
        targetY = py;
    });

    card.addEventListener('pointerleave', () => {
        targetX = 50;
        targetY = 50;
    });

    function update() {
        currentX += (targetX - currentX) * lerp;
        currentY += (targetY - currentY) * lerp;

        const centerX = currentX - 50;
        const centerY = currentY - 50;

        card.style.setProperty('--pointer-x', `${currentX}%`);
        card.style.setProperty('--pointer-y', `${currentY}%`);
        card.style.setProperty('--pointer-from-center', Math.hypot(centerX, centerY) / 50);
        card.style.setProperty('--pointer-from-top', currentY / 100);
        card.style.setProperty('--pointer-from-left', currentX / 100);

        // Tilt amount
        card.style.setProperty('--rotate-x', `${-centerX / 3.5}deg`);
        card.style.setProperty('--rotate-y', `${centerY / 3.5}deg`);

        // Bg shimmer position
        card.style.setProperty('--background-x', `${35 + (currentX * 0.3)}%`);
        card.style.setProperty('--background-y', `${35 + (currentY * 0.3)}%`);

        requestAnimationFrame(update);
    }

    // Start in center
    targetX = 50; targetY = 50;
    currentX = 50; currentY = 50;
    update();
}

// ── Scroll reveal ─────────────────────────────────────────────────
window.setupAnimations = () => {
    initHoloTilt(); // Initialize the holo engine
    const obs = new IntersectionObserver(entries => {
        entries.forEach(e => {
            if (e.isIntersecting) { e.target.classList.add('visible'); obs.unobserve(e.target); }
        });
    }, { threshold: 0.15 });

    document.querySelectorAll('.fade-in-up').forEach(el => obs.observe(el));
};

// ── Skill bars animate on scroll ──────────────────────────────────
window.animateSkillBars = () => {
    const obs = new IntersectionObserver(entries => {
        entries.forEach(e => {
            if (e.isIntersecting) {
                e.target.querySelectorAll('.skill-bar').forEach(bar => {
                    const w = bar.style.width;
                    bar.style.width = '0';
                    setTimeout(() => { bar.style.width = w; }, 100);
                });
                obs.unobserve(e.target);
            }
        });
    }, { threshold: 0.3 });

    document.querySelectorAll('.skills-section').forEach(el => obs.observe(el));
};
