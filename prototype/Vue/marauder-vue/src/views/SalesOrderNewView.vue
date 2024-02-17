<template>
    <div>
        <h1>Create a new Sales Order</h1>
        <p>Please select a Customer, and press Create Order.</p>
        <customer-search @customer-search="updateRoute" :type="searchType" :field="searchField" :value="searchValue"
            :select-action="{title: 'Create order', route: 'SalesOrderCreate'}"
        ></customer-search>
    </div>
</template>

<script>
import CustomerSearch from '@/components/CustomerSearch.vue'
export default {
    name: "SalesOrderNewView",
    components: {
        CustomerSearch,
    },
    data() {
        return {
            searchField: '',
            searchType: '',
            searchValue: '',
        };
    },
    computed: {
    },
    methods: {
        updateRoute(search) {
            this.$router.replace({ name: 'SalesOrderNew', query: { searchType: search.type, searchField: search.field, searchValue: search.value } });
        },
    },
    created() {
        // Fill in search parameters from route
        this.searchType = this.$route.query.searchType || 'contains';
        this.searchField = this.$route.query.searchField || 'customerName';
        this.searchValue = this.$route.query.searchValue || '';
    },
};

</script>

<style src="@/css/grid-table.css" scoped />
<style src="@/css/skeleton.css" scoped />
<style scoped>
#results-table {
    margin: 0 100px;
}

@media screen and (max-width: 1024px) {
    #results-table {
        margin: 0 80px;
    }
}

@media screen and (max-width: 768px) {
    #results-table {
        margin: 0 30px;
    }
}
</style>