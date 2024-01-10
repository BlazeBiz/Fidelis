<template>
    <div id="sales-order-view">
      <sales-order-form :sales-order-id="salesOrderId"/>
    </div>
  </template>
  
  <script>
  import SalesOrderForm from "../components/SalesOrderForm.vue";
  export default {
    components: {
      SalesOrderForm,
    },
    name: "SalesOrderEditView",
    data() {
      return {
        salesOrderId: null,
      };
    },
    created() {
        this.salesOrderId = parseInt(this.$route.params.id);
    },
  };
  </script>
  <style scoped>
  </style>