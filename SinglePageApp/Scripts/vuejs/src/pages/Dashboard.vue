<template>
  <v-container>
    <v-layout row wrap>
      <v-flex xs12 sm8 md6>
        <h1>{{ $t('welcomeMsg') }}</h1>
        <p>
          <i18n path="guideTxt">
            <br place="break">
            <a href="https://cli.vuejs.org/" target="_blank" place="action">{{$t('guideDocTxt')}}</a>
          </i18n>
        </p>
        <h3>{{$t('pluginsTxt')}}</h3>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
export default {
  data() {
    return {
      
    };
  },
};
</script>