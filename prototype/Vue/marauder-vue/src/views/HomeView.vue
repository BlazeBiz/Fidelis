<template>
    <div>
        <h1>Home</h1>
        <p>
            Welcome to Iron Marauder, a sample application. The demo is intended to illustrate some
            concepts and techniques around building a full-stack application with a .NET API backend and
            Vue.js with JavaScript in the frontend, deployed in Azure and running against an
            Azure SQL database.
        </p>
        <p>
            <strong>Note:</strong> This application uses the Azure SQL Serverless Free tier. One of the features
            of this, which can't be disabled, is the <em>auto-pause</em> feature. If there is no activity against the
            database for an hour, Azure SQL <em>pauses</em> the database. The first time the application tries to open a
            connection after this, Azure SQL must <em>resume</em> the database. This can take up to a minute, so it
            typically results in a connection timeout error. The API has retry logic to recover from the timeout and
            connect, but you'll notice a long delay the first time the application hits the database after a time
            of inactivity.
        </p>
        <h2>Database status</h2>
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