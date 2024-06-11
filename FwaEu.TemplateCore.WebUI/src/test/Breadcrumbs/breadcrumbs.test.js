/**
 * @vitest-environment jsdom
 */
import { mount, flushPromises  } from '@vue/test-utils';
import { createRouter, createWebHistory } from 'vue-router';
import Breadcrumbs from '@/Fwamework/Breadcrumbs/Components/BreadcrumbsComponent.vue';
import { describe, expect, it } from 'vitest';

const routes = [
    {
        name: 'path1',
        path: '/path1',
		meta: {
            breadcrumb: {
                title: "Node1"
            }
        }
    },
    {
        name: 'path2',
        path: '/path2',
        meta: {
            breadcrumb: {
                title: "Node2",
                parentName: 'path1'
            }
        }
    },
    {
        name: 'path3',
        path: '/path3',
        meta: {
            breadcrumb: {
                title: "Node3",
                parentName: 'path2'
            }
        }
    },
    {
        name: 'pathMissingMeta',
        path: '/pathMissingMeta'
    },
    {
        name: 'pathMissingParent',
        path: '/pathMissingParent',
        meta: {
            breadcrumb: {
                title: 'pathMissingParent',
                parentName: 'parentDoesNotExist'
            }
        }
    }
];
const $i18n = { t: (textKey) => textKey };

describe("Breadcrumbs", () => {
	const router = createRouter({
		history: createWebHistory(),
        routes
    });
	const wrapper = mount(Breadcrumbs, {
		global: {
			plugins: [router]
		},
        mocks: {
            $i18n
        }
    });

    it("Testing parent node resolution", async () => {

		await router.push({ name: 'path3' });
		await router.isReady()
		await flushPromises();

		await wrapper.vm.resolveBreadcrumb(router.currentRoute.value);

        expect(wrapper.vm.breadcrumbs[0].text).toBe('Node1');
        expect(wrapper.vm.breadcrumbs[1].text).toBe('Node2');
        expect(wrapper.vm.breadcrumbs[2].text).toBe('Node3');

        expect(wrapper.findAll('.breadcrumb-node').length).toBe(3);
    });

    it("Testing 'missing meta.breadcrumb'", async () => {
       
		await router.push({ name: 'pathMissingMeta' });
		await router.isReady()
		await flushPromises();

		await wrapper.vm.resolveBreadcrumb(router.currentRoute.value);

        expect(wrapper.vm.breadcrumbs[0].text).toBe('Missing "meta.breadcrumb"');
    });

    it("Testing 'node not found'", async () => {

		router.push({ name: 'pathMissingParent' });
		await router.isReady()
		await flushPromises();
		
		await wrapper.vm.resolveBreadcrumb(router.currentRoute.value);

        expect(wrapper.vm.breadcrumbs[0].text).toEqual(expect.stringContaining('Not found node'));
    });
});