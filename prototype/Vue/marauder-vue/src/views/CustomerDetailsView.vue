<template>
    <div id="customer-view">
      <customer-details :customer-id="customerId" />
    </div>
  </template>
  
  <script>
  import CustomerDetails from "../components/CustomerDetails.vue";
  export default {
    components: {
      CustomerDetails,
    },
    name: "CustomerDetailsView",
    data() {
      return {
        customerId: null,
      };
    },
    created() {
        this.customerId = parseInt(this.$route.params.id);
    },
  };
  </script>
  <style scoped>
  </style>