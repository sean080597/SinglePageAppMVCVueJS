<template>
  <v-card class="mb-2">
    <v-card-title primary-title style="padding:10px 16px">
      <v-layout row wrap>
        <v-checkbox v-model="model.checked" hide-details class="shrink mr-2"></v-checkbox>
        <v-flex>
          <router-link :to="'/department/employee/' + model.DEPARTMENT_ID">
            <h3 class="headline mb-0">{{model.DEPARTMENT_NAME}}</h3>
          </router-link>
          <div class="grey--text">{{model.DEPARTMENT_HEAD}}</div>
        </v-flex>
        <v-spacer></v-spacer>
        <v-btn color="primary" fab small dark :to="'/department/edit/' + model.DEPARTMENT_ID">
          <v-icon>edit</v-icon>
        </v-btn>
      </v-layout>
    </v-card-title>
  </v-card>
</template>

<script>
export default {
  props: {
    value: {
      type: Object,
      required: true
    }
  },
  computed: {
    model: {
      get() {
        return this.value;
      },
      set(v) {
        this.$emit("input", v);
      }
    }
  }
};
</script>