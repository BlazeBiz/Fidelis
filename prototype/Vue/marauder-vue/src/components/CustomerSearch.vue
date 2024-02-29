<template>
    <div>
        <div id="searchForm">
            <fieldset>
                <label for="searchValue">Find customers where field </label>
                <select name="searchField" id="searchField" v-model="search.field">
                    <option value="customerName" selected>Customer name</option>
                    <option value="customerNumber">Customer number</option>
                </select>&nbsp;
                <select name="searchType" id="searchType" v-model="search.type">
                    <option value="contains">Contains</option>
                    <option value="startsWith">Starts with</option>
                    <option value="equals">Equals</option>
                </select>&nbsp;
                <input type="search" name="searchValue" id="searchValue" v-model="search.value" />&nbsp;
                <button type="button" @click="searchCustomers">Find customers</button>
            </fieldset>
        </div>

        <div id="resultsArea" v-if="showResults">
            <h1>Results</h1>
            <div id="results-table" class="gridtable gridtable-5">
                <div class="col hdg">
                    Customer name
                </div>
                <div class="col hdg">
                    Customer number
                </div>
                <div class="col hdg">
                    Payment terms
                </div>
                <div class="col hdg">
                    GL link
                </div>
                <!-- Action column only if this isn't a selectable list -->
                <div class="col hdg" v-if="isSelectable()"></div>
                <div class="col hdg" v-else>Actions</div>
                <!-- **************** Skeleton ************************ -->
                <template v-if="isLoading">
                    <template v-for="n in 3" :key="n">
                        <div class="col skeleton skeleton-text"></div>
                        <div class="col skeleton skeleton-text"></div>
                        <div class="col skeleton skeleton-text"></div>
                        <div class="col skeleton skeleton-text"></div>
                        <div class="col skeleton skeleton-text"></div>
                    </template>
                </template>
                <template v-else>
                    <template v-for="c in customers" :key="c.customerId" style="cursor: pointer;">
                        <div :class="rowClass(c)" @click="onRowClick(c)">
                            <!-- Customer is hot only if this isn't a selectable list -->
                            <span v-if="isSelectable()">{{ c.customerName }}</span>
                            <router-link v-else :to="{ name: 'CustomerDetails', params: { id: c.customerId } }">
                                {{ c.customerName }}
                            </router-link>
                        </div>
                        <div :class="rowClass(c)" @click="onRowClick(c)">{{ c.customerNumber }}</div>
                        <div :class="rowClass(c)" @click="onRowClick(c)">{{ c.paymentTerms }}</div>
                        <div :class="rowClass(c)" @click="onRowClick(c)">{{ c.GLLink }}</div>
                        <div :class="rowClass(c)" @click="onRowClick(c)">
                            <!-- Show Actions only if this isn't a selectable list -->
                            <template v-if="!isSelectable()">
                                <router-link :to="{ name: 'CustomerDetails', params: { id: c.customerId } }"
                                    title="View details">
                                    <font-awesome-icon icon="fa-solid fa-binoculars" />
                                </router-link>
                                &nbsp;
                                <router-link :to="{ name: 'CustomerEdit', params: { id: c.customerId } }"
                                    title="Edit customer">
                                    <font-awesome-icon icon="fa-solid fa-pencil" />
                                </router-link>
                                &nbsp;
                                <router-link :to="{ name: 'SalesOrderCreate', params: { id: c.customerId } }"
                                    title="New sales order for customer">
                                    <font-awesome-icon icon="fa-solid fa-cart-plus" />
                                </router-link>
                            </template>
                        </div>
                    </template>
                </template>
                <div id="results-summary">{{ customers.length }} record{{ customers.length === 1 ? '' : 's' }} found.</div>
            </div>
        </div>

        <!-- Show the action button if this is a selectable list -->
        <button v-if="selectAction && selectAction.title && selectedCustomer" type="button" id="action"
            @click="takeAction()">{{ selectAction.title }}</button>

    </div>
</template>

<script>
import apiService from '@/services/apiService.js'
export default {
    name: "CustomerSearch",
    props: {
        field: '',
        type: '',
        value: '',
        selectAction: null, // .title and .route
    },
    components: {
    },
    data() {
        return {
            search: {
                field: '',
                type: '',
                value: ''
            },
            isLoading: false,
            showResults: false,
            customers: [],
            selectedCustomer: null,
        };
    },
    methods: {
        searchCustomers() {
            console.log("searchCustomers()");
            if (this.search.value.length === 0) {
                alert('Please specify a search value');
                return;
            }
            this.isLoading = true;
            this.showResults = true;
            apiService.searchCustomers(this.search.field, this.search.type, this.search.value).then(resp => {
                this.isLoading = false;
                this.customers = resp.data;
                // Update the route
                this.$emit('customer-search', this.search);
                // this.$router.replace({ name: 'CustomerSearch', query: { searchType: this.search.type, searchField: this.search.field, searchValue: this.search.value } });
            });
        },
        takeAction() {
            //SalesOrderCreate
            this.$router.push({ name: this.selectAction?.route, params: { id: this.selectedCustomer?.customerId } })
        },
        onRowClick(customer) {
            if (this.isSelectable()) {
                this.selectedCustomer = customer;
            }
        },
        isSelectable() {
            return this.selectAction && this.selectAction.title;
        },
        rowClass(customer) {
            return {
                col: true,
                selectable: this.isSelectable(),
                selected: customer.customerId === this.selectedCustomer?.customerId
            };
        },
    },
    created() {
        // Fill in search parameters from routexxx
        this.search.type = this.type || 'contains';
        this.search.field = this.field || 'customerName';
        this.search.value = this.value || '';

        if (this.search.value) {
            this.searchCustomers();
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

div.gridtable>div.col.selected {
    background-color: yellow;
    font-weight: bold;
}

div.gridtable>div.col.selectable {
    cursor: pointer;
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