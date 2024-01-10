<template>
    <div>
        <h1>Search Sales Orders</h1>
        <div id="searchForm">
            <fieldset>
                <label for="searchValue">Find sales orders where </label>
                <select name="searchField" id="searchField" v-model="searchField">
                    <option value="salesOrderNumber" selected>Sales order number</option>
                    <option value="customerPONumber" selected>Customer PO number</option>
                    <option value="customerNumber" selected>Customer number</option>
                    <option value="customerName" selected>Customer name</option>
                    <option value="customerId" selected>Customer id</option>
                </select>
                &nbsp;
                <select name="searchType" id="searchType" v-model="searchType">
                    <option value="contains">Contains</option>
                    <option value="startsWith">Starts with</option>
                    <option value="equals">Equals</option>
                </select>
                &nbsp;
                <input type="search" name="searchValue" id="searchValue" v-model="searchValue" />
                &nbsp;
                <button type="button" @click="search">Find sales orders</button>
            </fieldset>
        </div>

        <div id="resultsArea" v-if="showResults">
            <h1>Results</h1>
            <div id="results-table" class="gridtable gridtable-6">
                <div class="col hdg">
                    Id
                </div>
                <div class="col hdg">
                    Order number
                </div>
                <div class="col hdg">
                    Order date
                </div>
                <div class="col hdg">
                    Customer name
                </div>
                <div class="col hdg">
                    Customer PO number
                </div>
                <div class="col hdg">
                    Actions
                </div>
                <!-- **************** Skeleton ************************ -->
                <template v-if="isLoading">
                    <template v-for="n in 3" :key="n">
                        <div class="col skeleton skeleton-text"></div>
                        <div class="col skeleton skeleton-text"></div>
                        <div class="col skeleton skeleton-text"></div>
                        <div class="col skeleton skeleton-text"></div>
                        <div class="col skeleton skeleton-text"></div>
                        <div class="col skeleton skeleton-text"></div>
                    </template>
                </template>
                <template v-else>
                    <template v-for="ord in orders" :key="ord.customerId">
                        <div class="col">
                            <router-link :to="{ name: 'SalesOrderDetails', params: { id: ord.salesOrderId } }">
                                {{ ord.salesOrderId }}
                            </router-link>
                        </div>
                        <div class="col">{{ ord.salesOrderNumber }}</div>
                        <div class="col">{{ formatDate(ord.orderDate) }}</div>
                        <div class="col">{{ ord.customer.customerName }}</div>
                        <div class="col">{{ ord.customerPONumber }}</div>
                        <div class="col">
                            <router-link :to="{ name: 'SalesOrderDetails', params: { id: ord.salesOrderId } }"
                                title="View details">
                                <font-awesome-icon icon="fa-solid fa-binoculars" />
                            </router-link>
                            &nbsp;
                            <router-link :to="{ name: 'SalesOrderEdit', params: { id: ord.salesOrderId } }"
                                title="Edit sales order">
                                <font-awesome-icon icon="fa-solid fa-pencil" />
                            </router-link>
                        </div>
                    </template>
                </template>
            </div>
            <div id="results-summary">{{ orders.length }} record{{ orders.length === 1 ? '' : 's' }} found.</div>
        </div>
    </div>
</template>

<script>
import apiService from '@/services/apiService.js'
import utility from '@/services/utility.js'
export default {
    name: "SalesOrderSearchView",
    components: {
    },
    data() {
        return {
            isLoading: false,
            searchField: '',
            searchType: '',
            searchValue: '',
            showResults: false,
            orders: [],
        };
    },
    computed: {
    },
    methods: {
        isNumeric(n) {
            return !isNaN(parseInt(n)) && isFinite(n);
        },
        search() {
            if (this.searchValue.length === 0) {
                alert('Please specify a search value');
                return;
            }
            if (this.searchField === 'customerId') {
                if (!this.isNumeric(this.searchValue)) {
                    alert('Customer Id must be an integer value');
                    return;
                }
                this.searchType = 'equals';
            }
            this.isLoading = true;
            this.showResults = true;
            apiService.searchSalesOrders(this.searchField, this.searchType, this.searchValue).then(resp => {
                this.isLoading = false;
                this.orders = resp.data;
                // Update the route
                this.$router.replace({ name: 'SalesOrderSearch', query: { searchType: this.searchType, searchField: this.searchField, searchValue: this.searchValue } });
            });
        },
        formatDate(date) {
            return utility.formatDate(date);
        },

    },
    created() {
        // Fill in search parameters from routexxx
        this.searchType = this.$route.query.searchType || 'contains';
        this.searchField = this.$route.query.searchField || 'customerName';
        this.searchValue = this.$route.query.searchValue || '';

        if (this.searchValue) {
            this.search();
        }

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