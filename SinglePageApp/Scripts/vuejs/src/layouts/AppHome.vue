<template>
    <div>
        <toolbar></toolbar>
        <router-view :key="$route.fullPath"></router-view>
        <app-footer></app-footer>
        <vue-progress-bar></vue-progress-bar>
    </div>
</template>

<script>
import toolbar from '../components/Toolbar'
import AppFooter from '../components/AppFooter'

export default {
    components: {toolbar, AppFooter}
}
</script>

<style scoped>

</style>
