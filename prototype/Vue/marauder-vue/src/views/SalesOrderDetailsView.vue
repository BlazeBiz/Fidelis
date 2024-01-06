<template>
    <div id="sales-order-view">
      <sales-order-details :salesOrderId="salesOrderId" />
    </div>
  </template>
  
  <script>
  import SalesOrderDetails from "../components/SalesOrderDetails.vue";
  export default {
    components: {
      SalesOrderDetails,
    },
    name: "SalesOrderDetailsView",
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