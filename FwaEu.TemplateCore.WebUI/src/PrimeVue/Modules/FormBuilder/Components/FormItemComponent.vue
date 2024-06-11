<script>
    export default {
        props: {
            name: String,
            dataField: String,
            label: Object,
            editorOptions: Object,
            modelValue: {}
        },
        data(){
            return {
                currentPropsState: JSON.stringify(this.$props)
            }
        },
        watch:{
            '$props':{
                deep:true,
                handler(newProps){
                    const newPropsState = JSON.stringify(newProps);
                    if( newPropsState !== this.currentPropsState){
                        this.$parent.loadFormItemsFromSlot();
                        this.currentPropsState = newPropsState;
                    }
                }
            }
        },
        render(){
            //NOTE: This is a declarative component, it is used to allow form items declaration on template, maybe in the future it will work as a real component
            return null;
        }
    }
</script>