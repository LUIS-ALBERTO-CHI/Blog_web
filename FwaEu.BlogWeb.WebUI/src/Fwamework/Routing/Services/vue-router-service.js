import { createRouter, createWebHashHistory } from 'vue-router';

const router = createRouter({
	history: createWebHashHistory(),
	routes:[]
});

router.afterEach(() => {
	setTimeout(function () {
		window.scrollTo({
			top: 0,
			left: 0,
			behavior: 'smooth'
		});
	}, 0);
});
export default router;

