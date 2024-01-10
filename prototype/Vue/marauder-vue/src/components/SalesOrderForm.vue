<template>
  <template v-if="error">
    <h1>Error</h1>
    <p>{{ error.msg }}</p>
  </template>
  <div v-else id="salesOrder-details">
    <h1>
      <em>
        <span v-if="isLoading" id="skeleton-salesOrder-name" class="skeleton skeleton-text"></span>
        <span v-else>{{ (mode === 'new') ? 'New sales order' : 'Order ' + salesOrder.salesOrderNumber }}</span>
      </em>
    </h1>
    <section id="main-grid">
      <div class="gridtable standard-form gridlines" id="salesOrder-form">
        <!-- Show the id if it's an existing salesOrder -->
        <template v-if="mode === 'edit'">
          <div class="col">Sales Order id:</div>
          <div class="col" v-if="isLoading">
            <div class="skeleton skeleton-text"></div>
          </div>
          <div class="col" v-else>{{ salesOrder.salesOrderId }}</div>
        </template>
        <div class="col">Order number:</div>
        <div class="col" v-if="isLoading">
          <div class="skeleton skeleton-text"></div>
        </div>
        <div class="col" v-else>
          <input type="text" name="salesOrder-nbr" id="salesOrder-nbr" v-model="salesOrder.salesOrderNumber">
        </div>

        <div class="col">Order date:</div>
        <div class="col" v-if="isLoading">
          <div class="skeleton skeleton-text"></div>
        </div>
        <div class="col" v-else>{{ formatDate(salesOrder.orderDate) }}</div>

        <div class="col">Status:</div>
        <div class="col" v-if="isLoading">
          <div class="skeleton skeleton-text"></div>
        </div>
        <div class="col" v-else>
          <input type="text" name="salesOrder-status" id="salesOrder-status"
            v-model.number="salesOrder.salesOrderStatusCode">
        </div>

        <div class="col">Payment terms:</div>
        <div class="col" v-if="isLoading">
          <div class="skeleton skeleton-text"></div>
        </div>
        <div class="col" v-else>
          <input type="text" name="salesOrder-pt" id="salesOrder-pt" v-model.number="salesOrder.paymentTermsCode">
        </div>



        <!-- Show created and modified information if it's an existing salesOrder -->
        <template v-if="mode === 'edit'">
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
        </template>
      </div>
    </section>

    <section id="customer-info">
      <h2 v-if="isLoading" class="skeleton skeleton-text"></h2>
      <div v-else>
        <h2>Customer: {{ salesOrder?.customer?.customerName }}</h2>
        <!-- <router-link :to="{ name: 'CustomerDetails', params: { id: salesOrder?.customerId } }"
          title="View customer details">
          <font-awesome-icon icon="fa-solid fa-binoculars" /> Customer details
        </router-link> -->
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
        <div class="col" v-else>
          <input type="text" name="salesOrder-po" id="salesOrder-po" v-model="salesOrder.customerPONumber">
        </div>

        <div class="col">Ship to:</div>
        <div class="col" v-if="isLoading">
          <div class="skeleton skeleton-text"></div>
        </div>
        <div class="col" v-else>
          <select name="ship-to" id="ship-to" v-model="salesOrder.shipToAddressId">
            <option v-for="addr in salesOrder.customer.customerAddresses.filter(a => a.shipToFlag)"
              :key="addr.customerAddressId" :value="addr.customerAddressId">{{ addressForDropdown(addr) }}</option>
          </select>
        </div>

        <div class="col">Bill to:</div>
        <div class="col" v-if="isLoading">
          <div class="skeleton skeleton-text"></div>
        </div>
        <div class="col" v-else>
          <select name="ship-to" id="ship-to" v-model="salesOrder.billToAddressId">
            <option v-for="addr in salesOrder.customer.customerAddresses.filter(a => a.billToFlag)"
              :key="addr.customerAddressId" :value="addr.customerAddressId">{{ addressForDropdown(addr) }}</option>
          </select>
        </div>
      </div>

    </section>
    <div id="buttons-section">
      <button type="button" id="save" @click="save()"><font-awesome-icon icon="fa-regular fa-floppy-disk" /> Save</button>
      <button type="button" id="cancel" @click="reset()"><font-awesome-icon icon="fa-solid fa-ban" /> Cancel</button>
    </div>
  </div>
</template>
  
<script>
import apiService from '@/services/apiService.js'
import utility from "@/services/utility.js";
export default {
  components: {
  },
  name: "SalesOrderForm",
  props: {
    salesOrderId: {
      type: Number,
      required: true,
    },
    customerId: {
      type: Number,
    },
  },
  data() {
    return {
      salesOrder: this.newSalesOrder(),
      isLoading: false,
      mode: 'new',
      error: null,
    };
  },
  methods: {
    newSalesOrder() {
      return {
        customer: {
          customerAddresses: [],
        },
      };
    },
    fetchCustomer() {
      this.isLoading = true;
      this.customer = {};
      apiService.getCustomerById(this.customerId).then(resp => {
        this.isLoading = false;
        this.salesOrder.customer = resp.data;
        this.salesOrder.customerId = this.salesOrder.customer.customerId;
      })
        .catch(error => {
          this.isLoading = false;
          this.error = error;
        });
    },
    fetchSalesOrder() {
      this.isLoading = true;
      this.salesOrder = {};
      apiService.getSalesOrderById(this.salesOrderId).then(resp => {
        this.isLoading = false;
        this.salesOrder = resp.data;
      })
        .catch(error => {
          this.isLoading = false;
          this.error = error;
        });
    },
    save() {
      if (this.mode === 'new') {
        // Insert the salesOrder
        this.isLoading = true;
        apiService.insertSalesOrder(this.salesOrder).then(resp => {
          this.isLoading = false;
          this.salesOrder = resp.data;
          this.mode = 'edit';
        })
      } else {
        // edit
        this.isLoading = true;
        apiService.updateSalesOrder(this.salesOrder).then(resp => {
          this.isLoading = false;
          this.salesOrder = resp.data;
        })
          .catch(error => {
            this.isLoading = false;
            this.error = error;
          });
      }
    },
    reset() {
      if (this.mode === 'new') {
        this.salesOrder = this.newSalesOrder();
      } else {
        this.fetchSalesOrder();
      }
    },
    formatDate(date) {
      return utility.formatDate(date);
    },
    formatDateTime(date) {
      return utility.formatDateTime(date);
    },
    addressForDropdown(add) {
      let a = add.addressLine1;
      if (add.addressLine2) a += " | " + add.addressLine2;
      if (add.addressLine3) a += " | " + add.addressLine3;
      a += ` | ${add.city}, ${add.stateCode} ${add.zipCode}`;
      return a;
    },
    newAddress() {
      if (!this.salesOrder) return;
      if (!this.salesOrder.salesOrderAddresses) this.salesOrder.salesOrderAddresses = [];
      let newAddress = {
        salesOrderAddressId: 0,
        salesOrderId: this.salesOrder.salesOrderId,
        addressLine1: '',
        addressLine2: null,
        addressLine3: null,
        city: null,
        stateCode: '',
        zipCode: '',
        billToFlag: true,
        shipToFlag: true,
        shipToName: null,
        isDeleted: false,
      };
      this.salesOrder.salesOrderAddresses.push(newAddress);
    },
    removeAddress(add) {
      if (add.salesOrderAddressId) {
        // It exists in the database, mark it deleted
        add.isDeleted = true;
      } else {
        // It was added to the array locally but not saved yet. Remove it from the array.
        let index = this.salesOrder.salesOrderAddresses.indexOf(add);
        if (index >= 0) {
          this.salesOrder.salesOrderAddresses.splice(index, 1);
        }
      }
    }
  },
  created() {
    if (this.salesOrderId > 0) {
      this.mode = 'edit';
      this.fetchSalesOrder();
    } else {
      if (this.customerId > 0) {
        this.fetchCustomer();
      } else {
        // For a new order (salesOrderId = 0), there must be a customer id!
        this.error = {
          msg: 'ERROR: Customer Id must be included when calling for a new Sales Order.'
        }
      }
    }
  },

};
</script>

<style src="@/css/form.css" scoped />
<style src="@/css/grid-table.css" scoped />
<style src="@/css/skeleton.css" scoped />

<style scoped>
/* Grid column widths */
#address-table.gridtable.gridtable-6 {
  grid-template-columns: 1fr minmax(150px, auto) 120px 120px 150px 80px;
  margin: 0 100px;
}

#new-address-link {
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

#skeleton-salesOrder-name {
  width: 300px;
}

h1 {
  font-style: italic;
  color: var(--heading-fg-2);
}

div#buttons-section {
  margin-top: 10px;
}

button#save svg {
  color: rgb(22, 196, 22);
}

button#cancel svg {
  color: red;
}

@media screen and (max-width: 1200px) {
  section#main-grid {
    grid-template-columns: 1fr;
    margin: 20px 10px 10px 10px;
    gap: 0px;
  }
}
</style>