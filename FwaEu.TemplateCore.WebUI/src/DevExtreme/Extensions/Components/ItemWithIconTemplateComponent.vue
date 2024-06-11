<template>
   <div class="custom-item">
		<span :class="icon" />
		{{ text }}
	</div>
</template>
<script>
    export default {
        props: {
			icon: {
				type: String
			},
			text: {
				type: String
			}
		}
    };
</script>
