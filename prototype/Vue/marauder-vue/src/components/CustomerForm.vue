<template>
  <template v-if="error">
    <h1>Error</h1>
    <!-- <div v-if="error?.err?.response?.status === 404">
      <p>No customer with ID {{ customerId }} was found in the database.</p>
    </div>
    <div v-else> -->
    <p>{{ error.msg }}</p>
    <!-- </div> -->
  </template>
  <div v-else id="customer-details">
    <h1>
      <em>
        <span v-if="isLoading" id="skeleton-customer-name" class="skeleton skeleton-text"></span>
        <span v-else>{{ (mode === 'new') ? 'New customer' : customer.customerName }}</span>
      </em>
    </h1>
    <section id="main-grid">
      <div class="gridtable standard-form gridlines" id="customer-form">
        <!-- Show the id if it's an existing customer -->
        <template v-if="mode === 'edit'">
          <div class="col">Customer id:</div>
          <div class="col" v-if="isLoading">
            <div class="skeleton skeleton-text"></div>
          </div>
          <div class="col" v-else>{{ customer.customerId }}</div>
        </template>
        <div class="col">Customer name:</div>
        <div class="col" v-if="isLoading">
          <div class="skeleton skeleton-text"></div>
        </div>
        <div class="col" v-else>
          <input type="text" name="customer-name" id="customer-name" v-model="customer.customerName">
        </div>
        <div class="col">Customer number:</div>
        <div class="col" v-if="isLoading">
          <div class="skeleton skeleton-text"></div>
        </div>
        <div class="col" v-else>
          <input type="text" name="customer-nbr" id="customer-nbr" v-model="customer.customerNumber">
        </div>


        <div class="col">Payment terms:</div>
        <div class="col" v-if="isLoading">
          <div class="skeleton skeleton-text"></div>
        </div>
        <div class="col" v-else>
          <input type="text" name="customer-nbr" id="customer-nbr" v-model="customer.paymentTerms">
        </div>
        <div class="col">G/L Link:</div>
        <div class="col" v-if="isLoading">
          <div class="skeleton skeleton-text"></div>
        </div>
        <div class="col" v-else>
          <input type="text" name="customer-nbr" id="customer-nbr" v-model="customer.glLink">
        </div>

        <!-- Show created and modified information if it's an existing customer -->
        <template v-if="mode === 'edit'">
          <div class="col">Created on:</div>
          <div class="col" v-if="isLoading">
            <div class="skeleton skeleton-text"></div>
          </div>
          <div class="col" v-else>{{ formatDateTime(customer.created) }} by {{ customer.createdBy }}</div>
          <div class="col">Modified on:</div>
          <div class="col" v-if="isLoading">
            <div class="skeleton skeleton-text"></div>
          </div>
          <div class="col" v-else>{{ formatDateTime(customer.modified) }} by {{ customer.modifiedBy }}</div>
        </template>
      </div>
    </section>
    <section id="address-grid">
      <h2>Addresses</h2>
      <a id="new-address-link" href="#" @click.prevent="newAddress" title="Add an address">
        <font-awesome-icon icon="fa-solid fa-plus" /> New
      </a>
      <div class="gridtable gridtable-6" id="address-table">
        <div class="col hdg">
          <p>Address</p>
        </div>
        <div class="col hdg">
          <p>City</p>
        </div>
        <div class="col hdg">
          <p>State</p>
        </div>
        <div class="col hdg">
          <p>ZIP</p>
        </div>
        <div class="col hdg">
          <p>Type</p>
        </div>
        <div class="col hdg">
          <p></p>
        </div>
        <template v-for="add in customer.customerAddresses">
          <template v-if="!add.isDeleted">
            <div class="col">
              <input type="text" v-model="add.addressLine1" />
              <input type="text" v-model="add.addressLine2" />
              <input type="text" v-model="add.addressLine3" />
            </div>
            <div class="col">
              <input type="text" v-model="add.city" />
            </div>
            <div class="col">
              <input type="text" v-model="add.stateCode" />
            </div>
            <div class="col">
              <input type="text" v-model="add.zipCode" />
            </div>
            <div class="col">
              <input type="checkbox" v-model="add.shipToFlag" /> Shipping <br />
              <input type="checkbox" v-model="add.billToFlag" /> Billing
            </div>
            <div class="col">
              <a href="#" @click.prevent="removeAddress(add)" title="Remove address">
                <font-awesome-icon icon="fa-regular fa-trash-can" />
              </a>
            </div>
          </template>
        </template>
      </div>
    </section>
    <div id="buttons-section">
      <button type="button" @click="save()"><font-awesome-icon icon="fa-regular fa-floppy-disk" /> Save</button>
      <button type="button" @click="reset()"><font-awesome-icon icon="fa-solid fa-ban" /> Cancel</button>
    </div>
  </div>
</template>
  
<script>
import apiService from '@/services/apiService.js'
import utility from "@/services/utility.js";
export default {
  components: {
  },
  name: "CustomerForm",
  props: {
    customerId: {
      type: Number,
      required: true,
    },
  },
  data() {
    return {
      customer: this.newCustomer(),
      isLoading: false,
      mode: 'new',
      error: null,
    };
  },
  methods: {
    newCustomer() {
      return {
        customerAddresses: [],
      };
    },
    fetchCustomer() {
      this.isLoading = true;
      this.customer = {};
      apiService.getCustomerById(this.customerId).then(resp => {
        this.isLoading = false;
        this.customer = resp.data;
      })
        .catch(error => {
          this.isLoading = false;
          this.error = error;
        });
    },
    save() {
      if (this.mode === 'new') {
        // Insert the customer
        this.isLoading = true;
        apiService.insertCustomer(this.customer).then(resp => {
          this.isLoading = false;
          this.customer = resp.data;
          this.mode = 'edit';
        })
      } else {
        // edit
        this.isLoading = true;
        apiService.updateCustomer(this.customer).then(resp => {
          this.isLoading = false;
          this.customer = resp.data;
        })
          .catch(error => {
            this.isLoading = false;
            this.error = error;
          });
      }
    },
    reset() {
      if (this.mode === 'new') {
        this.customer = this.newCustomer();
      } else {
        this.fetchCustomer();
      }
    },
    formatDateTime(date) {
      return utility.formatDateTime(date);
    },
    addressHTML(add) {
      let a = add.addressLine1;
      if (add.addressLine2) a += "<br />" + add.addressLine2;
      if (add.addressLine3) a += "<br />" + add.addressLine3;
      return a;
    },
    newAddress() {
      if (!this.customer) return;
      if (!this.customer.customerAddresses) this.customer.customerAddresses = [];
      let newAddress = {
        customerAddressId: 0,
        customerId: this.customer.customerId,
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
      this.customer.customerAddresses.push(newAddress);
    },
    removeAddress(add) {
      if (add.customerAddressId) {
        // It exists in the database, mark it deleted
        add.isDeleted = true;
      } else {
        // It was added to the array locally but not saved yet. Remove it from the array.
        let index = this.customer.customerAddresses.indexOf(add);
        if (index >= 0) {
          this.customer.customerAddresses.splice(index, 1);
        }
      }
    }
  },
  created() {
    if (this.customerId > 0) {
      this.mode = 'edit';
      this.fetchCustomer();
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

div#customer-details {
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

#skeleton-customer-name {
  width: 300px;
}

h1 {
  font-style: italic;
  color: var(--heading-fg-2);
}

div#buttons-section {
  margin-top: 10px;
}

@media screen and (max-width: 1200px) {
  section#main-grid {
    grid-template-columns: 1fr;
    margin: 20px 10px 10px 10px;
    gap: 0px;
  }
}
</style>