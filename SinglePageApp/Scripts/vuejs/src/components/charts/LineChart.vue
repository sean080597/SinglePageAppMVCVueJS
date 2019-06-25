<template>
  <apexcharts type="line" :options="chartOptions" :series="series"></apexcharts>
</template>

<script>
import VueApexCharts from 'vue-apexcharts'

export default {
  name: 'LineChart',
  components: {
    apexcharts: VueApexCharts,
  },
  data: function() {
    return {
      chartOptions: {
        xaxis: {
          type: 'numeric'
        },
        yaxis:{
          opposite: true,
          axisBorder: {
            show: true,
            color: '#78909C',
            offsetX: 0,
            offsetY: 0
          },
        },
        chart: {
          zoom: {
            enabled: false
          },
          animations: {
            enabled: false
          },
        },
        stroke: {
          width: [5, 5, 4],
          curve: 'straight'
        },
        title: {
          text: 'Missing data (null values)'
        }
      },
      series: [{
        name: "Session Duration",
        data: [45, 52, 38, 24, 33, 26, 21, 20, 6, 8, 15, 10, 52, 38, 24, 33, 26, 21, 20, 6, 8, 15, 10, 52, 38, 24, 33, 26, 21, 20, 6, 8, 15, 10, 52, 38, 24, 33, 26, 21, 20, 6, 8, 15, 10, 52, 38, 24, 33, 26, 21, 20, 6, 8, 15, 10, 52, 38, 24, 33, 26, 21, 20, 6, 8, 15, 10, 52, 38, 24, 33, 26, 21, 20, 6, 8, 15, 10, 52, 38, 24, 33, 26, 21, 20, 6, 8, 15, 10, 52, 38, 24, 33, 26, 21, 20, 6, 8, 15, 10, 52, 38, 24, 33, 26, 21, 20, 6, 8, 15, 10]
      },
      {
        name: "Page Views",
        data: [35, 41, 62, 42, 13, 18, 29, 37, 36, 51, 32, 35, 52, 38, 24, 33, 26, 21, 20, 6, 8, 15, 10, 52, 41, 62, 42, 13, 18, 29, 37, 36, 51, 32, 35, 52, 41, 62, 42, 13, 18, 29, 37, 36, 51, 32, 35, 52, 41, 62, 42, 13, 18, 29, 37, 36, 51, 32, 35, 52]
      },
      {
        name: 'Total Visits',
        data: [87, 57, 74, 99, 75, 38, 62, 47, 82, 56, 45, 47]
      }],
    }
  },
}
</script>
