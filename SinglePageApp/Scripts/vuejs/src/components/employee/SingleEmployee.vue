<template>
  <v-card class="mb-2">
    <v-card-title primary-title style="padding:10px 16px">
      <v-layout row wrap>
        <v-checkbox v-model="model.checked" hide-details class="shrink mr-2"></v-checkbox>
        <v-flex>
          <v-layout row wrap align-center>
            <h3 class="headline mb-0">{{model.EMPLOYEE_NAME}}</h3>
            <h5 class="title blue-grey--text">&nbsp;- {{model.DEPARTMENT.DEPARTMENT_NAME}}</h5>
          </v-layout>
          <div class="grey--text">{{model.EMPLOYEE_SALARY}}</div>
        </v-flex>
      </v-layout>
      <v-spacer></v-spacer>
      <v-btn color="primary" fab small dark :to="'/employee/edit/' + model.EMPLOYEE_ID">
        <v-icon>edit</v-icon>
      </v-btn>
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