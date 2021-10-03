<template>
  <v-container grid-list-md>
    <v-layout row wrap justify-center>
      <TitleLink title="Department's list"></TitleLink>
      <!-- SidebarTop show in small screen-->
      <v-flex xs12 sm10 hidden-md-and-up>
        <SidebarTOP></SidebarTOP>
      </v-flex>

      <!-- List Departments -->
      <v-flex xs10 sm8 md6>
        <v-flex v-if="departments.length > 0">
          <SingleDepartment
            v-for="(depart, index) in departments"
            :key="depart.DEPARTMENT_ID"
            v-model="departments[index]"
          ></SingleDepartment>
        </v-flex>
        <v-flex v-else>
          <h1 class="grey--text">No Department</h1>
        </v-flex>
      </v-flex>

      <!-- SideBar show in large screen -->
      <v-flex md4 hidden-sm-and-down>
        <Sidebar cate="Department" v-model="calcIsChecked" @delSelected="destroySubmit"></Sidebar>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
import TitleLink from "../../components/TitleLink";
import SingleDepartment from "../../components/department/SingleDepartment";
import Sidebar from "../../components/Sidebar";
import SidebarTOP from "../../components/SidebarTOP";

export default {
  components: { SingleDepartment, Sidebar, SidebarTOP, TitleLink },
  data() {
    return {
      departments: []
    };
  },
  methods: {
    loadDepartments() {
      this.$Progress.start();
      axios
        .get("/api/department")
        .then(
          ({ data }) => (
            $.each(data, function(indexInArray, valueOfElement) {
              data[indexInArray]["checked"] = false;
            }),
            this.departments = data,
            this.$Progress.finish()
          )
        )
        .catch(error => console.log(error.response.data));
    },
    destroySubmit() {
      Swal.fire({
        title: "Do you wanna delete these records?",
        text: "Cannot restore these records!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Delete!"
      }).then(result => {
        if (result.value) {
          this.selectedDepartments.forEach(element => {
            axios
              .delete("/api/department/" + element.DEPARTMENT_ID)
              .then(() => {
                //set event to reload faculties
                Fire.$emit("ReloadDepartments");
                if (result.value) {
                  Swal.fire(
                    "Deleted!",
                    "Deleted records successfully.",
                    "success"
                  );
                  this.$Progress.finish();
                }
              })
              .catch(() => {
                Swal.fire("Failed!", "Đã có lỗi xảy ra!", "warning");
                this.$Progress.fail();
              });
          });
        }
      });
    }
  },
  computed: {
    calcIsChecked() {
      return this.departments.some(depart => {
        return depart.checked === true;
      });
    },
    selectedDepartments() {
      return this.departments.filter(item => item.checked);
    }
  },
  created() {
    this.loadDepartments();

    Fire.$on("ReloadDepartments", () => {
      this.loadDepartments();
    });
  },
};
</script>

<style scoped>
</style>
