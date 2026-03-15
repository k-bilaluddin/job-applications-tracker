<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <div id="app">
    <header>
      <div class="container">
        <h1>Job Application Tracker</h1>
        <nav>
          <RouterLink to="/applications">Applications</RouterLink>
        </nav>
      </div>
    </header>

    <main>
      <RouterView />
    </main>
  </div>
</template>

<style scoped>
#app {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

header {
  background-color: #3498db;
  color: white;
  padding: 1rem 0;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 2rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

h1 {
  margin: 0;
  font-size: 1.5rem;
  font-weight: 600;
}

nav {
  display: flex;
  gap: 1rem;
}

nav a {
  color: white;
  text-decoration: none;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  transition: background-color 0.2s;
}

nav a:hover,
nav a.router-link-active {
  background-color: rgba(255, 255, 255, 0.2);
}

main {
  flex: 1;
  background-color: #f8f9fa;
  padding: 0;
}

@media (max-width: 768px) {
  .container {
    flex-direction: column;
    gap: 1rem;
    text-align: center;
  }

  nav {
    justify-content: center;
  }
}
</style>