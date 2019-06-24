<template>
  <v-container grid-list-md>
    <v-layout row wrap align-center justify-center>
      <TitleLink title="Department/Edit"></TitleLink>
      <!-- List employees -->
      <v-flex xs8 sm10 md6>
        <v-form @submit.prevent="editDepartment">
          <v-text-field
            label="Department Name"
            type="text"
            v-model="form.DEPARTMENT_NAME"
            v-validate="'required|max:50'"
            :error-messages="errors.collect('Department Name')"
            data-vv-name="Department Name"
            required
          ></v-text-field>
          <v-text-field
            label="Department Head"
            type="text"
            v-model="form.DEPARTMENT_HEAD"
            v-validate="'required|max:30'"
            :error-messages="errors.collect('Department Head')"
            data-vv-name="Department Head"
            required
          ></v-text-field>

          <v-btn color="primary" type="submit">Edit</v-btn>
        </v-form>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
import TitleLink from "../../components/TitleLink";

export default {
  components: { TitleLink },
  $_veeValidate: {
    validator: "new"
  },
  data() {
    return {
      departID: this.$route.params.department_id,
      form: {},
      departments: [],
      dictionary: {
        custom: {
          "Department Name": {
            required: () => "Name can not be empty",
            max: "The name field may not be greater than 50 characters"
          }
        }
      }
    };
  },
  methods: {
    loadDepartment() {
      axios
        .get("/api/department/" + this.departID)
        .then(({ data }) => {
          this.form = data;
        })
        .catch(() => {
          Swal("Failed!", "Some error occurred", "warning");
          this.$Progress.fail();
        });
    },
    editDepartment() {
      this.$Progress.start();
      this.$validator.validateAll().then(result => {
        if (result) {
          axios
            .put("/api/department/" + this.departID, this.form)
            .then(res => {
              this.$router.push("/department");

              toast.fire({
                type: "success",
                title: "Edited Department successfully"
              });

              this.$Progress.finish();
            })
            .catch(() => {
              Swal("Failed!", "Some error occurred", "warning");
              this.$Progress.fail();
            });
        } else {
          this.$Progress.fail();
        }
      });
    }
  },
  created() {
    this.loadDepartment()
  },
  mounted() {
    this.$validator.localize("en", this.dictionary);
  }
};
</script>