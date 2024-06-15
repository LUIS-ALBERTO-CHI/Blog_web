const DataGridDevExtremSingleLookupColumns = () => import('@/Samples/DataGrid/DevExtreme/Components/DataGridWithSingleLookupColumnsComponent.vue');
const DataGridDevExtremBatchedLookupColumns = () => import('@/Samples/DataGrid/DevExtreme/Components/DataGridWithBatchedLookupColumnsComponent.vue');

export default
[
	{
		path: '/DataGrid/SingleMasterDataRequestDevExtreme',
		name: 'SampleDataGridDevExtremWithSingleLookupColumns',
		component: DataGridDevExtremSingleLookupColumns,
		meta: {
			breadcrumb: {
				title: 'DevExtreme DataGrid with single lookup columns',
				parentName: 'default'
			}
		}
	},
	{
		path: '/DataGrid/BatchedMasterDataRequestDevExtreme',
		name: 'SampleDataGridDevExtremWithBatchedLookupColumns',
		component: DataGridDevExtremBatchedLookupColumns,
		meta: {
			breadcrumb: {
				title: 'DevExtreme DataGrid with batched lookup columns',
				parentName: 'default'
			}
		}
	}
]