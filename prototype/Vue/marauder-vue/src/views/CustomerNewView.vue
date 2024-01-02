<template>
    <div id="customer-view">
      <customer-form :customer-id="0"/>
    </div>
  </template>
  
  <script>
  import CustomerForm from "../components/CustomerForm.vue";
  export default {
    components: {
      CustomerForm,
    },
    name: "CustomerNewView",
    data() {
      return {
      };
    },
  };
  </script>
  <style scoped>
  </style>