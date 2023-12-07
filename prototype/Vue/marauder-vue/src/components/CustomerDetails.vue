<template>
  <div id="customer-details">
    <h1>Customer Details</h1>
    <h2>{{ customer.customerName }}</h2>
    <section id="main-grid">
        <div class="gridtable standard-form gridlines shadow" id="customer-form">
          <div>Customer id:</div>
          <div>{{ customer.customerID }}</div>
          <div>Customer name:</div>
          <div>{{ customer.customerName }}</div>
          <div>Customer number:</div>
          <div>{{ customer.customerNbr }}</div>
        </div>
        <br />
        <!-- =====================================================  -->
    </section>
  </div>
</template>
  
<script>
import apiService from '/services/apiService.js'
export default {
  components: {
  },
  name: "CustomerDetails",
  props: {
    customerId: {
      type: Number,
      required: true,
    },
  },
  data() {
    return {
      customer: {},
      isLoading: false,
    };
  },
  methods: {
    refreshPage() {
      this.isLoading = true;
      apiService.getCustomerById(this.customerId).then(resp => {
        this.isLoading = false;
        this.customer = resp.data;
      });

    }
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
/* Grid column widths */
#status-table.gridtable.gridtable-2 {
  grid-template-columns: 1fr 180px;
}

#milestone-table.gridtable.gridtable-4 {
  grid-template-columns: 1fr 120px 180px 180px;
}

#milestone-table.gridtable.gridtable-6 {
  grid-template-columns: 35px 1fr 120px 180px 180px 50px;
}

#risk-table.gridtable.gridtable-4 {
  grid-template-columns: auto auto auto 180px;
}

#risk-table.gridtable.gridtable-6 {
  grid-template-columns: 35px 2fr 1fr 2fr 180px 50px;
}

#milestone-table-title,
#risk-table-title {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
}

section.project-info {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  justify-content: flex-start;
  column-gap: 40px;
}
section.project-info > div {
  display: inline-block;
}

section.project-info .label {
  font-weight: bold;
  font-size: 0.9rem;
  color: var(--body-text-fg);
}

section.project-info .field {
  font-size: 0.9rem;
  color: var(--heading-fg);
}

section#main-grid {
  margin: 20px 200px 10px 200px;
}

.gridtable-title {
  font-size: 0.9em;
  font-weight: bold;
  padding-left: 5px;
}

.gridtable {
  font-size: 0.9em;
}

#project-form {
  margin-bottom: 25px;
}
a {
  font-weight: bold;
}

.psuedo-row {
  min-height: 25px;
}

div.fas.fa-grip-vertical {
  border: 1px solid black;
  padding: 4px;
  border-right: none;
  border-radius: 5px 0 0 5px;
  display: flex;
  justify-content: center;
  align-items: center;
  cursor: grab;
}

div.fas.fa-grip-vertical:active {
  cursor: grabbing;
}

div.col.drag-over {
  border-top: 6px solid deepskyblue;
}

#health-table-narrow,
#health-table-narrow-spacer {
  display: none;
}

@media screen and (max-width: 1200px) {
  section#main-grid {
    grid-template-columns: 1fr;
    margin: 20px 10px 10px 10px;
    gap: 0px;
  }

  #health-table-wide,
  #health-table-wide-spacer {
    display: none;
  }

  #health-table-narrow {
    display: grid;
  }

  #health-table-narrow-spacer {
    display: initial;
  }
}
</style>