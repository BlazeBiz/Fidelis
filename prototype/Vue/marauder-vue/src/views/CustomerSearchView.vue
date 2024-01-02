<template>
    <div>
        <h1>Search Customers</h1>
        <router-link :to="{ name: 'CustomerNew' }">New customer</router-link>

        <div id="searchForm">
            <fieldset>
                <label for="searchValue">Find customers where field </label>
                <select name="searchField" id="searchField" v-model="searchField">
                    <option value="name" selected>Customer Name</option>
                    <option value="nbr">Customer Number</option>
                </select>&nbsp;
                <select name="searchType" id="searchType" v-model="searchType">
                    <option value="contains">Contains</option>
                    <option value="startsWith">Starts with</option>
                    <option value="equals">Equals</option>
                </select>&nbsp;
                <input type="search" name="searchValue" id="searchValue" v-model="searchValue" />&nbsp;
                <button type="button" @click="search">Find customers</button>
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
                <!-- <div class="col col-center hdg"><img src="@/assets/images/GreenButton.png" /></div>
      <div class="col col-center hdg"><img src="@/assets/images/AmberButton.png" /></div>
      <div class="col col-center hdg"><img src="@/assets/images/RedButton.png" /></div> -->
                <div class="col hdg">
                    Payment terms
                </div>
                <div class="col hdg">
                    GL link
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
                        <!-- <div class="col col-center skeleton skeleton-text"></div> -->
                    </template>
                </template>
                <template v-else>
                    <template v-for="c in customers" :key="c.customerId">
                        <div class="col">
                            <router-link :to="{ name: 'CustomerDetails', params: { id: c.customerId } }">
                                {{ c.customerName }}
                            </router-link>
                        </div>
                        <div class="col">{{ c.customerNumber }}</div>
                        <div class="col">{{ c.paymentTerms }}</div>
                        <div class="col">{{c.GLLink}}</div>
                        <div class="col">
                            <!-- TODO: font-awesome icons here -->
                            <router-link :to="{ name: 'CustomerDetails', params: { id: c.customerId } }">view</router-link>
                            | <router-link :to="{ name: 'CustomerEdit', params: { id: c.customerId } }">edit</router-link>
                        </div>
                    </template>
                </template>
            </div>




            <div id="results-summary">{{ customers.length }} record{{ customers.length === 1 ? '' : 's' }} found.</div>
        </div>
    </div>
</template>

<script>
import apiService from '@/services/apiService.js'
export default {
    name: "CustomerSearchView",
    components: {
    },
    data() {
        return {
            isLoading: false,
            searchField: '',
            searchType: '',
            searchValue: '',
            showResults: false,
            customers: [],
        };
    },
    computed: {
    },
    methods: {
        search() {
            if (this.searchValue.length === 0) {
                alert('Please specify a search value');
                return;
            }
            this.isLoading = true;
            this.showResults = true;
            apiService.searchCustomers(this.searchField, this.searchType, this.searchValue).then(resp => {
                this.isLoading = false;
                this.customers = resp.data;
                // Update the route
                this.$router.replace({name: 'CustomerSearch', query: { searchType: this.searchType, searchField: this.searchField, searchValue: this.searchValue}});
            });
        },
    },
    created() {
        // Fill in search parameters from routexxx
        this.searchType = this.$route.query.searchType || 'contains';
        this.searchField = this.$route.query.searchField || 'name';
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