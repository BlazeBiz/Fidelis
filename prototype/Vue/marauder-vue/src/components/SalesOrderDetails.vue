<template>
  <template v-if="error">
    <h1>Error</h1>
    <div v-if="error.err.response.status === 404">
      <p>No sales order with ID {{ salesOrderId }} was found in the database.</p>
    </div>
    <div v-else>
      <p>An error occurred reading sales order ID {{ salesOrderId }}: {{ error.msg }}</p>
    </div>
  </template>
  <div v-else id="salesOrder-details">
    <h1 v-if="isLoading" class="skeleton skeleton-text"></h1>
    <h1 v-else>Sales order: {{ salesOrder.salesOrderNumber }}</h1>

    <router-link v-if="salesOrder?.salesOrderId" :to="{ name: 'SalesOrderEdit', params: { id: salesOrder.salesOrderId } }" title="Edit sales order">
      <font-awesome-icon icon="fa-solid fa-pencil" /> Edit
    </router-link>

    <section id="main-grid">
      <div class="gridtable standard-form gridlines" id="salesOrder-form">

        <div class="col">Sales Order id:</div>
        <div class="col" v-if="isLoading">
          <div class="skeleton skeleton-text"></div>
        </div>
        <div class="col" v-else>{{ salesOrder.salesOrderId }}</div>

        <div class="col">Order number:</div>
        <div class="col" v-if="isLoading">
          <div class="skeleton skeleton-text"></div>
        </div>
        <div class="col" v-else>{{ salesOrder.salesOrderNumber }}</div>

        <div class="col">Order date:</div>
        <div class="col" v-if="isLoading">
          <div class="skeleton skeleton-text"></div>
        </div>
        <div class="col" v-else>{{ formatDate(salesOrder.orderDate) }}</div>


        <!-- TODO: Status codes -->
        <div class="col">Status:</div>
        <div class="col" v-if="isLoading">
          <div class="skeleton skeleton-text"></div>
        </div>
        <div class="col" v-else>{{ salesOrder.salesOrderStatusCode }}</div>

        <!-- TODO: Payment terms codes -->
        <div class="col">Payment terms:</div>
        <div class="col" v-if="isLoading">
          <div class="skeleton skeleton-text"></div>
        </div>
        <div class="col" v-else>{{ salesOrder.paymentTermsCode }}</div>

        <!--
        <div class="col">G/L Link:</div>
        <div class="col" v-if="isLoading">
          <div class="skeleton skeleton-text"></div>
        </div>
        <div class="col" v-else>{{ salesOrder.glLink }}</div>
      -->
        <div class="col">Created on:</div>
        <div class="col" v-if="isLoading">
          <div class="skeleton skeleton-text"></div>
        </div>
        <div class="col" v-else>{{ formatDateTime(salesOrder.created) }} by {{ salesOrder.createdBy }}</div>

        <div class="col">Modified on:</div>
        <div class="col" v-if="isLoading">
          <div class="skeleton skeleton-text"></div>
        </div>
        <div class="col" v-else>{{ formatDateTime(salesOrder.modified) }} by {{ salesOrder.modifiedBy }}</div>
      </div>
    </section>

    <h2 v-if="isLoading" class="skeleton skeleton-text"></h2>
    <div v-else>
      <h2>Customer: {{ salesOrder?.customer?.customerName }}</h2>
      <router-link :to="{ name: 'CustomerDetails', params: { id: salesOrder?.customerId } }"
        title="View customer details">
        <font-awesome-icon icon="fa-solid fa-binoculars" /> Customer details
      </router-link>
    </div>
    <div class="gridtable standard-form gridlines" id="customer-form">

      <div class="col">Customer name:</div>
      <div class="col" v-if="isLoading">
        <div class="skeleton skeleton-text"></div>
      </div>
      <div class="col" v-else>{{ salesOrder.customer.customerName }}</div>

      <div class="col">Customer number:</div>
      <div class="col" v-if="isLoading">
        <div class="skeleton skeleton-text"></div>
      </div>
      <div class="col" v-else>{{ salesOrder.customer.customerNumber }}</div>

      <div class="col">PO number:</div>
      <div class="col" v-if="isLoading">
        <div class="skeleton skeleton-text"></div>
      </div>
      <div class="col" v-else>{{ salesOrder.customerPONumber }}</div>

      <div class="col">Ship to:</div>
      <div class="col" v-if="isLoading">
        <div class="skeleton skeleton-text"></div>
      </div>
      <div class="col" v-else v-html="addressHTML(salesOrder.shipToAddress)"></div>

      <div class="col">Bill to:</div>
      <div class="col" v-if="isLoading">
        <div class="skeleton skeleton-text"></div>
      </div>
      <div class="col" v-else v-html="addressHTML(salesOrder.billToAddress)"></div>

    </div>
  </div>
</template>
  
<script>
import apiService from '@/services/apiService.js'
import utility from "@/services/utility.js";
export default {
  components: {
  },
  name: "SalesOrderDetails",
  props: {
    salesOrderId: {
      type: Number,
      required: true,
    },
  },
  data() {
    return {
      salesOrder: {},
      isLoading: false,
      error: null,
    };
  },
  methods: {
    refreshPage() {
      this.isLoading = true;
      apiService.getSalesOrderById(this.salesOrderId).then(resp => {
        this.isLoading = false;
        this.salesOrder = resp.data;
      })
        .catch(error => {
          this.isLoading = false;
          this.error = error;
        });
    },
    formatDate(date) {
      return utility.formatDate(date);
    },
    formatDateTime(date) {
      return utility.formatDateTime(date);
    },
    addressHTML(add) {
      let a = add.addressLine1;
      if (add.addressLine2) a += "<br />" + add.addressLine2;
      if (add.addressLine3) a += "<br />" + add.addressLine3;
      a += `<br /> ${add.city}, ${add.stateCode} ${add.zipCode}`;
      return a;
    },
  },
  created() {
    this.refreshPage();
  },

};
</script>

<style src="@/css/form.css" scoped />
<style src="@/css/grid-table.css" scoped />
<style src="@/css/skeleton.css" scoped />

<style scoped>
h1 {
  font-style: italic;
  color: var(--heading-fg-2);
}

h2 {
  margin-bottom: 0;
}

/* Grid column widths */
#address-table.gridtable.gridtable-5 {
  grid-template-columns: 1fr minmax(150px, auto) 120px 120px 150px;
  margin: 0 100px;
}

div#salesOrder-details {
  min-width: 1000px;
}

section#main-grid {
  margin: 20px 200px 10px 200px;
}

.gridtable-title {
  font-size: 0.9em;
  font-weight: bold;
  padding-left: 5px;
}

a {
  font-weight: bold;
}

@media screen and (max-width: 1200px) {
  section#main-grid {
    grid-template-columns: 1fr;
    margin: 20px 10px 10px 10px;
    gap: 0px;
  }
}
</style>