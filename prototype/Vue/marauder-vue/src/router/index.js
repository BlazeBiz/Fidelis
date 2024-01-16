import { createRouter, createWebHistory } from 'vue-router';
import HomeView from '@/views/HomeView.vue';
import CustomerSearchView from '@/views/CustomerSearchView.vue';
import AboutView from '@/views/AboutView.vue';
import CustomerDetailsView from '@/views/CustomerDetailsView.vue';
import CustomerEditView from '@/views/CustomerEditView.vue';
import CustomerNewView from '@/views/CustomerNewView.vue';
import SalesOrderSearchView from '@/views/SalesOrderSearchView.vue';
import SalesOrderDetailsView from '@/views/SalesOrderDetailsView.vue';
import SalesOrderEditView from '@/views/SalesOrderEditView.vue';
import SalesOrderNewView from '@/views/SalesOrderNewView.vue';

const APP_NAME = 'Marauder';
const routes = [
    {
        path: '/',
        name: 'Home',
        component: HomeView,
        meta: {
            requiresAuth: false,
            title: 'Home'
        }
    },
    {
        path: '/customers',
        name: 'CustomerSearch',
        component: CustomerSearchView,
        meta: {
            requiresAuth: false,
            title: 'Customer search'
        }
    },
    {
        path: '/about',
        name: 'About',
        component: AboutView,
        meta: {
            requiresAuth: false,
            title: 'About'
        }
    },
    {
        path: '/customer/:id',
        name: 'CustomerDetails',
        component: CustomerDetailsView,
        meta: {
            requiresAuth: false,
            title: 'Customer details'
        }
    },
    {
        path: '/customer/new',
        name: 'CustomerNew',
        component: CustomerNewView,
        meta: {
            requiresAuth: false,
            title: 'Add customer'
        }
    },
    {
        path: '/customer/:id/edit',
        name: 'CustomerEdit',
        component: CustomerEditView,
        meta: {
            requiresAuth: false,
            title: 'Edit customer'
        }
    },
    {
        path: '/customer/:id/new-order',
        name: 'SalesOrderNew',
        component: SalesOrderNewView,
        meta: {
            requiresAuth: false,
            title: 'Add sales order'
        }
    },
    {
        path: '/salesorders',
        name: 'SalesOrderSearch',
        component: SalesOrderSearchView,
        meta: {
            requiresAuth: false,
            title: 'Sales order search'
        }
    },
    {
        path: '/salesorder/:id',
        name: 'SalesOrderDetails',
        component: SalesOrderDetailsView,
        meta: {
            requiresAuth: false,
            title: 'Sales order details'
        }
    },
    {
        path: '/salesorder/:id/edit',
        name: 'SalesOrderEdit',
        component: SalesOrderEditView,
        meta: {
            requiresAuth: false,
            title: 'Edit sales order'
        }
    },
];
const router = createRouter(
    {
        history: createWebHistory(),
        routes: routes
    });

router.afterEach((to/*, from*/) => {
    document.title = `${APP_NAME}: ${to.meta.title}`;
});

export default router;