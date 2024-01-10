<template>
    <div id="sales-order-view">
      <sales-order-form :sales-order-id="0" :customer-id="parseInt($route.params.id)"/>
    </div>
  </template>
  
  <script>
  import SalesOrderForm from "../components/SalesOrderForm.vue";
  export default {
    components: {
      SalesOrderForm,
    },
    name: "SalesOrderNewView",
    data() {
      return {
      };
    },
  };
  </script>
  <style scoped>
  </style>