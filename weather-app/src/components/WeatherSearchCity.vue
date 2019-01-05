<template>
  <div class="searchCityAutoComplete">
    <autocomplete
      :source="source"
      resultsValue="id"
      :showNoResults="false"
      :results-display="formatCityName"
      @selected="getTempratureInformationForCity"
      :resultsFormatter="resultsFormatter"
      :initialValue="defaultValue"
      :initialDisplay="defaultDisplay"
      placeholder="Type city name"
    ></autocomplete>
  </div>
</template>

<script>
import Autocomplete from "vuejs-auto-complete";

export default {
  name: "SearchCity",
  props: ["defaultValue", "defaultDisplay", "historyData", "source"],
  components: {
    Autocomplete
  },
  methods: {
    formatCityName(result) {
      return result.name + " - " + result.country;
    },
    getTempratureInformationForCity(selectedCity) {
      this.$emit("update", selectedCity);
    },
    resultsFormatter(data) {
      if (data && data.length < 1) {
        return this.historyData;
      } else {
        return data;
      }
    }
  }
};
</script>
<style>
.searchCityAutoComplete .autocomplete .autocomplete__box .autocomplete__icon {height: 30px;line-height: 30px;}
</style>
