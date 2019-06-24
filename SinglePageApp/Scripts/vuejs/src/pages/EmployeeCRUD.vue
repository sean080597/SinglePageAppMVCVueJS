<template>
  <div>
    <v-toolbar flat color="white">
      <v-toolbar-title>My Employee CRUD</v-toolbar-title>
      <v-divider class="mx-2" inset vertical></v-divider>
      <!-- New Item Button -->
      <v-dialog v-model="dialog" max-width="500px">
        <template v-slot:activator="{ on }">
          <v-btn color="primary" dark class="mb-2" v-on="on">New Item</v-btn>
        </template>
        <v-card>
          <v-card-title>
            <span class="headline">{{ formTitle }}</span>
          </v-card-title>

          <v-card-text>
            <v-container grid-list-md>
              <v-layout wrap>
                <v-flex xs12 sm6 md4>
                  <v-text-field
                    v-model="editedItem.EMPLOYEE_NAME"
                    label="Employee name"
                    v-validate="'required|max:50'"
                    :error-messages="errors.collect('Employee Name')"
                    data-vv-name="Employee Name"
                    required
                  ></v-text-field>
                </v-flex>
                <v-flex xs12 sm6 md4>
                  <v-text-field
                    v-model="editedItem.EMPLOYEE_SALARY"
                    label="Salary"
                    v-validate="'required|numeric|max:6'"
                    :error-messages="errors.collect('Employee Salary')"
                    data-vv-name="Employee Salary"
                    onkeydown="return event.keyCode == 69 || event.keyCode == 189 ? false : true"
                    required
                  ></v-text-field>
                </v-flex>
                <v-flex xs12 sm6 md4>
                  <v-autocomplete
                    :items="departments"
                    item-text="DEPARTMENT_NAME"
                    item-value="DEPARTMENT_ID"
                    label="Department Name"
                    v-model="editedItem.EMPLOYEE_DEPARTMENT"
                    v-validate="'required'"
                    :error-messages="errors.collect('Department Name')"
                    data-vv-name="Department Name"
                    required
                  ></v-autocomplete>
                </v-flex>
              </v-layout>
            </v-container>
          </v-card-text>

          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="blue darken-1" flat @click="close">Cancel</v-btn>
            <v-btn color="blue darken-1" flat @click="save">Save</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>

      <!-- Delete Button -->
      <v-btn
        color="error"
        class="mb-2"
        :disabled="!calcIsChecked"
        @click="callDeleteSelected"
      >Delete</v-btn>
    </v-toolbar>
    <v-data-table
      :headers="headers"
      :items="employees"
      v-model="selected"
      item-key="EMPLOYEE_ID"
      select-all
      class="elevation-1"
    >
      <template v-slot:items="props">
        <td>
          <v-checkbox v-model="props.selected" primary hide-details></v-checkbox>
        </td>
        <td class="text-xs-left">{{ props.item.EMPLOYEE_NAME }}</td>
        <td class="text-xs-center">{{ props.item.EMPLOYEE_SALARY }}</td>
        <td class="text-xs-left">{{ props.item.DEPARTMENT.DEPARTMENT_NAME }}</td>
        <td class="layout px-4">
          <v-icon small class="mr-2" @click="editItem(props.item)">edit</v-icon>
          <v-icon small @click="callDeleteSingle(props.item)">delete</v-icon>
        </td>
      </template>
      <template v-slot:no-data>
        <v-btn color="primary" @click="initialize">Reset</v-btn>
      </template>
    </v-data-table>
  </div>
</template>

<script>
export default {
  $_veeValidate: {
    validator: "new"
  },
  data() {
    return {
      dialog: false,
      headers: [
        {
          text: "Employee Name",
          align: "left",
          value: "EMPLOYEE_NAME"
        },
        { text: "Salary", align: "center", value: "EMPLOYEE_SALARY" },
        { text: "Department Name", value: "EMPLOYEE_DEPARTMENT" },
        { text: "Actions", value: "name", sortable: false }
      ],
      selected: [],
      department_id: this.$route.params.department_id,
      departments: [],
      employees: [],
      editedIndex: -1,
      editedItem: {
        EMPLOYEE_ID: null,
        EMPLOYEE_NAME: null,
        EMPLOYEE_SALARY: null,
        EMPLOYEE_DEPARTMENT: null
      },
      defaultItem: {
        EMPLOYEE_ID: null,
        EMPLOYEE_NAME: null,
        EMPLOYEE_SALARY: null,
        EMPLOYEE_DEPARTMENT: null
      },
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

  computed: {
    formTitle() {
      return this.editedIndex === -1 ? "New Item" : "Edit Item";
    },
    calcIsChecked() {
      return this.selected.length > 0 ? true : false;
    }
  },

  watch: {
    dialog(val) {
      val || this.close();
    }
  },

  created() {
    this.initialize();
    Fire.$on("ReloadEmployees", () => {
      this.loadEmployees();
    });
  },

  mounted() {
    this.$validator.localize("en", this.dictionary);
  },

  methods: {
    getDepartments() {
      axios
        .get("/api/department")
        .then(({ data }) => (this.departments = data));
    },
    loadEmployees() {
      this.$Progress.start();
      if (this.department_id === "undefined" || this.department_id == null) {
        axios
          .get("/api/employee/all")
          .then(
            ({ data }) => ((this.employees = data), this.$Progress.finish())
          );
      } else {
        axios
          .get("/api/department/employee/" + this.department_id)
          .then(
            ({ data }) => ((this.employees = data), this.$Progress.finish())
          )
          .catch(error => console.log(error.response.data));
      }
    },

    initialize() {
      this.getDepartments();
      this.loadEmployees();
    },

    editItem(item) {
      this.editedIndex = this.employees.indexOf(item);
      this.editedItem = Object.assign({}, item);
      this.dialog = true;
    },

    // Handle Delete ============================================================
    deleteItem(empID) {
      this.$Progress.start();
      axios
        .delete("/api/employee/" + empID)
        .then(() => {
          //notify
          Swal.fire("Deleted!", "Deleted successfully.", "success");
          this.$Progress.finish();
          //reload departments
          Fire.$emit("ReloadEmployees");
        })
        .catch(() => {
          Swal.fire("Failed!", "Some Error Occurred!", "warning");
          this.$Progress.fail();
        });
    },

    callDeleteSingle(item) {
      Swal.fire({
        title: "Do you wanna delete this records?",
        text: "Cannot restore this records!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Delete!"
      }).then(result => {
        if (result.value) {
          this.deleteItem(item.EMPLOYEE_ID)
        }
      });
    },

    callDeleteSelected() {
      Swal.fire({
        title: "Do you wanna delete these records?",
        text: "Cannot restore these records!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Delete!"
      }).then(result => {
        this.selected.forEach(element => {
          this.deleteItem(element.EMPLOYEE_ID);
        });
      });
    },

    // =========================================================================
    close() {
      this.dialog = false;
      setTimeout(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      }, 300);
    },

    save() {
      this.$Progress.start();
      //request to server
      this.$validator.validateAll().then(result => {
        if (result) {
          if (this.editedIndex > -1) {
            axios
              .put(
                "/api/employee/" + this.editedItem.EMPLOYEE_ID,
                this.editedItem
              )
              .then(res => {
                toast.fire({
                  type: "success",
                  title: "Edited Department successfully"
                });

                //reload Employees
                Fire.$emit("ReloadEmployees");

                this.$Progress.finish();
                this.close();
              })
              .catch(() => {
                Swal("Failed!", "Some error occurred", "warning");
                this.$Progress.fail();
              });
          } else {
            axios
              .post("/api/employee", {
                EMPLOYEE_NAME: this.editedItem.EMPLOYEE_NAME,
                EMPLOYEE_SALARY: this.editedItem.EMPLOYEE_SALARY,
                EMPLOYEE_DEPARTMENT: this.editedItem.EMPLOYEE_DEPARTMENT
              })
              .then(res => {
                toast.fire({
                  type: "success",
                  title: "Created Employee successfully"
                });

                //reload Employees
                Fire.$emit("ReloadEmployees");

                this.$Progress.finish();
                this.close();
              })
              .catch(() => {
                Swal("Failed!", "Some error occurred", "warning");
                this.$Progress.fail();
              });
          }
        } else {
          this.$Progress.fail();
        }
      });
    }
  }
};
</script>