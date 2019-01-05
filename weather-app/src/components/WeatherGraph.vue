<template>
  <div class="weatherGraph">
    <line-chart :chartData="weatherChartData" :options="weatherChartOptions"></line-chart>
  </div>
</template>

<script>
import LineChart from "./LineChart";

export default {
  name: "WeatherGraph",
  components: {
    LineChart
  },
  props: ["weatherChartData", "weatherChartOptions"]
};
</script>

<style scoped>
.weatherGraph {border: 1px #d3d8d2 solid;border-radius: 4px;margin-bottom: 15px}
.weatherGraph:hover {box-shadow: 5px 0px 40px black}
</style>