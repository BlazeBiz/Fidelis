<template>
  <div id="nav-item" :class="(level) ? 'child-item' : ''">
    <div v-if="item.type === 2">
      <div id="child-item">
        <div v-if="item.display">{{ item.display }}</div>
        <nav-item v-for="child in item.contents" :item="child" :level="level+1"></nav-item>
      </div>

    </div>
    <div v-if="item.type === 1">
      <router-link id="child-item" :to="{ name: item.route }">{{ item.display }}</router-link>
    </div>
  </div>
</template>
  
<script>
import NavItem from '@/components/NavItem.vue'; // recursion
import apiService from '@/services/apiService.js'
import utility from "@/services/utility.js";
let TYPES = {
  ROUTE: 1,
  COLLECTION: 2,
};
let STATES = {
  EXPANDED: 1,
  COLLAPSED: 2,
};
export default {
  components: {
    // NavItem,
  },
  name: "NavItem",
  props: {
    item: {
      type: Object,
      required: true,
    },
    level: {
      type: Number,
      default: 0,
    }
  },
  data() {
    return {
    };
  },
  methods: {
  },
  created() {
  },

};
</script>

<!-- <style src="@/css/form.css" scoped />
<style src="@/css/grid-table.css" scoped />
<style src="@/css/skeleton.css" scoped /> -->

<style scoped>
.child-item {
  padding-left: 8px;
}

.router-link-exact-active {
  background-color: rgba(255,255, 255, 0.7);
  font-weight: bolder;
  border-top: 1px solid #646cff;
  border-bottom: 1px solid #646cff;
}

a:hover {
  color:brown;
}
</style>