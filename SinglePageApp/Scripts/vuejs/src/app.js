window._ = require('lodash');
window.Vue = require('vue')
window.Fire = new Vue()

import Vue from 'vue'
import router from './router';
import VueProgressBar from 'vue-progressbar';
import Vuetify from 'vuetify';
import VeeValidate from 'vee-validate';
import Swal from 'sweetalert2';
import {i18n} from './plugins/i18n'
import moment from 'moment'

//set globally axios
window.axios = require('axios');

//v-validate
Vue.use(VeeValidate)

//vuetify
Vue.use(Vuetify)

//vue progressbar
Vue.use(VueProgressBar, {
    color: '#0063ae',
    failedColor: 'red',
    thickness: '3px',
})
//Sweet Alert 2
window.Swal = Swal;
const toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timer: 3000
});
window.toast = toast;

//Filters
Vue.filter('dateYMD', function(val){
    return moment(new Date(val)).locale('en').format('YYYY/MM/DD');
});

Vue.filter('dateYMDHMS', function(val){
    return moment(new Date(val)).locale('en').format('YYYY/MM/DD HH:mm:ss');
});

Vue.filter('dateReformat', function(val){
    return val.substring(0, 4) + '/' + val.substring(4, 6) + '/' + val.substring(6, 8);
});

//vue components
Vue.component('AppHome', require('./layouts/AppHome.vue').default);

//new Vue
new Vue({
    i18n,
    el: '#app',
    router
})