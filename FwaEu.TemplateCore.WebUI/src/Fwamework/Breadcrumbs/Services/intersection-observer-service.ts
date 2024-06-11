import { ref, onMounted } from "vue";

type UseIntersectionParams<T> = {
	list: T;
	root?: {
		value: Element;
	};
};

function useIntersectionList<T>(params: UseIntersectionParams<T>) {
	const hiddenCount = ref(0);

	const elementsRef: Record<number, HTMLElement | null> = {};
	const observerRef = ref<IntersectionObserver>();

	function onIntersect(entries: IntersectionObserverEntry[]) {
		const elements = Object.entries(elementsRef)
			.filter(([, el]) => !!el && !!el.isConnected)
			.map(([index, el]) => ({ index: +index, el }));

		let level = hiddenCount.value;

		entries.forEach((entire) => {
			const el = elements.find((ref) => ref.el === entire.target);

			if (!el) {
				return;
			}

			if (entire.intersectionRatio < 1 && el.index + 1 > level) {
				level = el.index + 1;
			} else if (entire.intersectionRatio === 1 && el.index < level) {
				level = el.index;
			}
		});

		hiddenCount.value = elements.length === level ? level - 1 : level;
	}

	onMounted(() => {
		observerRef.value = new IntersectionObserver(onIntersect, {
			root: params.root ? params.root.value : undefined,
			threshold: 1
		});
	});

	const crumbRefFn = (element: HTMLElement, id: number) => {
		elementsRef[id] = element;

		if (observerRef.value && elementsRef[id]) {
			observerRef.value.observe(elementsRef[id] as HTMLElement);
		}
	};

	return { hiddenCount, crumbRefFn };
}

export { useIntersectionList };
