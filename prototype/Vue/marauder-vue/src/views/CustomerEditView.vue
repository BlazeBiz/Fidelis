<template>
    <div id="customer-view">
      <customer-edit :customer-id="customerId" />
    </div>
  </template>
  
  <script>
  import CustomerEdit from "../components/CustomerEdit.vue";
  export default {
    components: {
      CustomerEdit,
    },
    name: "CustomerEditView",
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