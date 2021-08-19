package com.example.proyecto_final.adapter

import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentManager
import androidx.lifecycle.Lifecycle
import androidx.viewpager2.adapter.FragmentStateAdapter
import com.example.proyecto_final.ui.fragments.MapFragment
import com.example.proyecto_final.ui.fragments.SettingsFragment

class ViewPagerFragmentAdapter(fragmentManager: FragmentManager, lifecycle: Lifecycle) :
    FragmentStateAdapter(fragmentManager, lifecycle) {
    companion object {
        const val MAX_TABS = 2
        const val FIRST_TAB = 0
        const val SECOND_TAB = 1
    }

    override fun getItemCount(): Int = MAX_TABS


    override fun createFragment(position: Int): Fragment {

        return when (position) {
            FIRST_TAB -> {
                MapFragment.newInstance()
            }
            SECOND_TAB -> {

                SettingsFragment.newInstance()
            }
            else ->
                MapFragment.newInstance()
        }
    }
}