<template>
  <v-toolbar>
    <v-toolbar-items>
      <v-btn flat>
        <v-icon right dark>create</v-icon>
        Create Employee
      </v-btn>
      <v-btn flat>
        <v-icon right dark>delete</v-icon>
        Delete Employee
      </v-btn>
    </v-toolbar-items>
  </v-toolbar>
</template>

<script>
export default {
    props: ['data'],
}
</script>