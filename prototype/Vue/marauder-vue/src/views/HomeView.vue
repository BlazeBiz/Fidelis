<template>
    <div>
        <h1>Home</h1>
        <p>
            Welcome to Iron Marauder, a sample application. The demo is intended to illustrate some concepts and
            techniques around building a full-stack application.<br />
            You can find more information, including a link to the source code, on the 
            <router-link :to="{name: 'About'}">About page</router-link>.
            </p>
        <h2>Database status</h2>
        <p>
            After a time of inactivity, the first connection to the database <em>may take up to a minute</em>. See the 
            <router-link :to="{name: 'About'}">About page</router-link>.
            for details. 
        </p>
        <hr />
        <p class="db-status" v-if="isLoading">
            <font-awesome-icon icon="fa-solid fa-rotate" class="fa-spin" />
            Connecting to the database...
            <font-awesome-icon icon="fa-solid fa-rotate" class="fa-spin" />
        </p>
        <div v-else>
            <p class="db-status-error" v-if="error">
                There was an error connecting to the database: {{ error }}
            </p>
            <p class="db-status" v-else>
                Connected to the database at {{ formatDateTime(lastUpdate) }}.<br />
                There are {{ statistics.numberCustomers }} customers, and {{ statistics.numberSalesOrders }} sales orders in
                the Marauder database.
            </p>
        </div>
    </div>
</template>

<script>
import apiService from '@/services/apiService.js'
import utility from "@/services/utility.js";
export default {
    components: {
    },
    name: "HomeView",
    data() {
        return {
            statistics: {},
            lastUpdate: null,
            isLoading: false,
            error: null,
        };
    },
    methods: {
        refreshPage() {
            this.isLoading = true;
            apiService.getStatistics().then(resp => {
                this.isLoading = false;
                this.statistics = resp.data;
                this.lastUpdate = new Date();
            })
                .catch(error => {
                    this.isLoading = false;
                    this.error = error;
                });
        },
        formatDateTime(date) {
            return utility.formatDateTime(date, false);
        },
    },
    created() {
        this.refreshPage();
    },
};
</script>

<style scoped>
p.db-status-error {
    font-size: 1.2rem;
    font-weight: bolder;
    color: red;
}

p.db-status {
    font-size: 1.2rem;
    color: green;
}
</style>