<template>
  <v-container grid-list-md>
    <v-layout row wrap align-center justify-center>
      <TitleLink title="Employee/Create"></TitleLink>
      <!-- List employees -->
      <v-flex xs8 sm10 md6>
        <v-form @submit.prevent="editEmployee">
          <v-text-field
            label="Employee Name"
            type="text"
            v-model="form.EMPLOYEE_NAME"
            v-validate="'required|max:50'"
            :error-messages="errors.collect('Employee Name')"
            data-vv-name="Employee Name"
            required
          ></v-text-field>
          <v-text-field
            label="Salary"
            type="number"
            v-model="form.EMPLOYEE_SALARY"
            v-validate="'required|numeric|max:6'"
            :error-messages="errors.collect('Employee Salary')"
            data-vv-name="Employee Salary"
            onkeydown="return event.keyCode == 69 || event.keyCode == 189 ? false : true"
            required
          ></v-text-field>
          <v-autocomplete
            :items="departments"
            item-text="DEPARTMENT_NAME"
            item-value="DEPARTMENT_ID"
            label="Department Name"
            v-model="form.EMPLOYEE_DEPARTMENT"
            v-validate="'required'"
            :error-messages="errors.collect('Department Name')"
            data-vv-name="Department Name"
            required
          ></v-autocomplete>

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
      empID: this.$route.params.employee_id,
      form: {},
      departments: [],
      dictionary: {
        custom: {
          "Employee Name": {
            required: () => "Name can not be empty",
            max: "The name field may not be greater than 50 characters"
          },
          "Department Name": {
            required: "Select field is required"
          }
        }
      }
    };
  },
  methods: {
    getDepartments() {
      axios
        .get("/api/department")
        .then(({ data }) => (this.departments = data));
    },
    loadEmployee() {
      axios
        .get("/api/employee/all/" + this.empID)
        .then(({ data }) => {
          this.form = data
        })
        .catch(() => {
          Swal("Failed!", "Some error occurred", "warning");
          this.$Progress.fail();
        });
    },
    editEmployee() {
      this.$Progress.start();
      this.$validator.validateAll().then(result => {
        if (result) {
          axios
            .put("/api/employee/" + this.empID, this.form)
            .then(res => {
              this.$router.push("/employee");

              toast.fire({
                type: "success",
                title: "Edited Employee successfully"
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
    this.getDepartments()
    this.loadEmployee()
  },
  mounted() {
    this.$validator.localize("en", this.dictionary)
  }
};
</script>