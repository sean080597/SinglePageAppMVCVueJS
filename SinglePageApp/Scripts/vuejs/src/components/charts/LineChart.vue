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
          categories: [51, 67, 70, 85, 91]
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
        data: [6, 7, 1, 1, 11]
      }],
    }
  },
}
</script>
