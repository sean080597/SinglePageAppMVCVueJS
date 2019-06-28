<template>
  <v-container grid-list-xs>
    <v-layout row wrap justify-center>
      <v-flex xs10 sm8 md6>
        <LineChart/>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
import LineChart from '../components/charts/LineChart'
export default {
  components:{
    LineChart
  },
  data() {
    return {
      
    }
  },

  methods: {
    
  },

  created() {
    
  },
}
</script>