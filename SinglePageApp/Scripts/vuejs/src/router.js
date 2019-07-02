import Vue from 'vue'
import VueRouter from 'vue-router';

//vue router
Vue.use(VueRouter)

const routes = [
    { path: '/', component: require('./pages/Dashboard.vue').default },
    { path: '/about', component: require('./pages/About.vue').default },
    { path: '/department', component: require('./pages/Department/Department.vue').default },
    { path: '/employee', component: require('./pages/Employee/Employee.vue').default },
    { path: '/department/employee/:department_id', component: require('./pages/Employee/Employee.vue').default },
    { path: '/department/create', component: require('./pages/Department/Create.vue').default },
    { path: '/department/edit/:department_id', component: require('./pages/Department/Edit.vue').default },
    { path: '/employee/create', component: require('./pages/Employee/Create.vue').default },
    { path: '/employee/edit/:employee_id', component: require('./pages/Employee/Edit.vue').default },
    { path: '/try_department', component: require('./pages/DepartmentCRUD.vue').default },
    { path: '/try_employee', component: require('./pages/EmployeeCRUD.vue').default },
    { path: '/chart', component: require('./pages/Chart.vue').default },
    { path: '/readwritebinfile', component: require('./pages/ReadWriteBin.vue').default },

    { path: '/svserver', component: require('./pages/SVServer.vue').default },
    { path: '/svsetting', component: require('./pages/SVSetting.vue').default },
];

const router = new VueRouter({
    mode: 'history', //removes # (hashtag) from url
    base: '/',
    fallback: true, //router should fallback to hash (#) mode when the browser does not support history.pushState
    routes // short for `routes: routes`
});

export default router;