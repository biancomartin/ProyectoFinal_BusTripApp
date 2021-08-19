package com.example.proyecto_final.ui.fragments

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import com.example.proyecto_final.MainActivity
import com.example.proyecto_final.R
import com.example.proyecto_final.adapter.ViewPagerFragmentAdapter.Companion.FIRST_TAB
import com.example.proyecto_final.ui.MenuListItem
import kotlinx.android.synthetic.main.fragment_settings.*
import kotlinx.android.synthetic.main.view_item_menu.view.*

class SettingsFragment : Fragment() {
    private val menuItemViewRemovedLinesSelected by lazy { menu_item_view_remove_lines }
    private val menuItemSetBusLines by lazy { menu_item_set_bus_lines }
    private val menuItemAlgorithms by lazy { menu_item_view_algorithms }

    private lateinit var menuBusLinesList: List<MenuListItem>
    private lateinit var menuAlgorithmsList: List<MenuListItem>

    companion object {

        fun newInstance() = SettingsFragment()

    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.fragment_settings, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        menuItemViewRemovedLinesSelected.view_settings_menu_big_item_text_view_label.text =
            getString(R.string.settings_menu_item_removed_lines_saved)
        menuItemSetBusLines.view_settings_menu_big_item_text_view_label.text =
            getString(R.string.settings_menu_item_view_history)
        menuItemAlgorithms.view_settings_menu_big_item_text_view_label.text =
            getString(R.string.settings_menu_item_algorithms)

        menuItemAlgorithms.view_settings_menu_big_item_image_view_icon.setImageDrawable(
            ContextCompat.getDrawable(requireContext(), R.drawable.ic_algoritmo)
        )

        menuItemViewRemovedLinesSelected.view_settings_menu_big_item_image_view_icon.setImageDrawable(
            ContextCompat.getDrawable(requireContext(), R.drawable.ic_eliminar)
        )

        menuItemSetBusLines.view_settings_menu_big_item_image_view_icon.setImageDrawable(
            ContextCompat.getDrawable(requireContext(), R.drawable.ic_autobus_lines)
        )

        menuItemViewRemovedLinesSelected.setOnClickListener {
            removedLinesSelected()
        }

        menuBusLinesList = listOf(
            MenuListItem("500 - AMARILLO") { lineBusActionItem(500) },
            MenuListItem("501 - ROJO") { lineBusActionItem(501) },
            MenuListItem("502 - BLANCO") { lineBusActionItem(502) },
            MenuListItem("503 - AZUL") { lineBusActionItem(503) },
            MenuListItem("504 - VERDE") { lineBusActionItem(504) },
            MenuListItem("505 - MARRON") { lineBusActionItem(505) }
        )
        menuItemSetBusLines.setSubMenuList(menuBusLinesList)

        menuAlgorithmsList = listOf(
            MenuListItem(RegressionAlgType.REGRESSION_LINEAL_MULTIPLE.toString()) {
                showAlgorithmActionItem(
                    RegressionAlgType.REGRESSION_LINEAL_MULTIPLE.toString()
                )
            },
            MenuListItem(RegressionAlgType.REGRESSION_MATRIX_SCOPE.toString()) {
                showAlgorithmActionItem(
                    RegressionAlgType.REGRESSION_MATRIX_SCOPE.toString()
                )
            }
        )

        menuItemAlgorithms.setSubMenuList(menuAlgorithmsList)
    }

    private fun removedLinesSelected() {

        val fragment = parentFragmentManager.fragments[0]

        if (fragment is MapFragment) {
            fragment.mapFragmentViewModel.removedSelectedLinesByConfigurations()
        }

        Toast.makeText(
            context,
            getString(R.string.settings_menu_item_view_removed_lines_selected_toast),
            Toast.LENGTH_LONG
        ).show()
    }

    private fun lineBusActionItem(linea: Int) {
        setLineSelection(linea)
        Toast.makeText(
            context,
            getString(R.string.settings_menu_line_selection_text, linea.toString()),
            Toast.LENGTH_LONG
        ).show()
    }

    private fun showAlgorithmActionItem(algorithm: String) {

        val fragment = parentFragmentManager.fragments[0]

        if (fragment is MapFragment) {
            fragment.mapFragmentViewModel.setAlgorithmSelectedByConfigurations(algorithm)
        }

        Toast.makeText(
            context,
            getString(R.string.settings_menu_algorithm_selection_text, algorithm),
            Toast.LENGTH_SHORT
        ).show()
    }

    fun setLineSelection(linea: Int) {
        val parentActvity = activity as MainActivity
        parentActvity.swipeToTab(FIRST_TAB)
        val fragment = parentFragmentManager.fragments[0]

        if (fragment is MapFragment) {
            fragment.mapFragmentViewModel.setLineSelectedByConfigurations(linea)
        }
    }
}

enum class RegressionAlgType(val valor: String) {
    REGRESSION_LINEAL_MULTIPLE("Regresión Lineal Múltiple"),
    REGRESSION_MATRIX_SCOPE("Enfoque Matricial");

    override fun toString() = this.valor
}