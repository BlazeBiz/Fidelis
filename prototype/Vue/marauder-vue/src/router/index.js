import { createRouter, createWebHistory } from 'vue-router';
import HomeView from '@/views/HomeView.vue';
import CustomerSearchView from '@/views/CustomerSearchView.vue';
import AboutView from '@/views/AboutView.vue';
import CustomerDetailsView from '@/views/CustomerDetailsView.vue';
import CustomerEditView from '@/views/CustomerEditView.vue';
import CustomerNewView from '@/views/CustomerNewView.vue';
import SalesOrderSearchView from '@/views/SalesOrderSearchView.vue';
import SalesOrderDetailsView from '@/views/SalesOrderDetailsView.vue';

const routes = [
    { path: '/', name: 'Home', component: HomeView },
    { path: '/customers', name: 'CustomerSearch', component: CustomerSearchView },
    { path: '/about', name: 'About', component: AboutView },
    { path: '/customer/:id', name: 'CustomerDetails', component: CustomerDetailsView },
    { path: '/customer/:id/edit', name: 'CustomerEdit', component: CustomerEditView },
    { path: '/customer/new', name: 'CustomerNew', component: CustomerNewView },
    { path: '/salesorders', name: 'SalesOrderSearch', component: SalesOrderSearchView },
    { path: '/salesorder/new', name: 'SalesOrderNew', component: CustomerNewView },
    { path: '/salesorder/:id', name: 'SalesOrderDetails', component: SalesOrderDetailsView },
];
const router = createRouter(
    {
        history: createWebHistory(),
        routes: routes
    });
export default router;