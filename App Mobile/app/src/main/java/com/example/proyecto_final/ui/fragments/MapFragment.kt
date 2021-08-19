package com.example.proyecto_final.ui.fragments

import android.Manifest
import android.content.pm.PackageManager
import android.graphics.Color
import android.location.Location
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentTransaction
import com.example.proyecto_final.MainActivity
import com.example.proyecto_final.R
import com.example.proyecto_final.adapter.CustomInfoWindowAdapter
import com.example.proyecto_final.ui.*
import com.example.proyecto_final.utils.MapUtils
import com.example.proyecto_final.utils.ViewUtils
import com.example.proyecto_final.utils.ViewUtils.getBusColorRoute
import com.example.proyecto_final.utils.ViewUtils.getBusIcon
import com.example.proyecto_final.utils.invokeAlertDialog
import com.example.proyecto_final.utils.onClickThrottled
import com.example.domain.response.Coordinates
import com.example.domain.response.LineBus
import com.example.domain.response.RecorridoBaseInformation
import com.example.domain.response.TravelLineInformation
import com.getbase.floatingactionbutton.FloatingActionButton
import com.google.android.gms.location.LocationServices
import com.google.android.gms.maps.CameraUpdateFactory
import com.google.android.gms.maps.GoogleMap
import com.google.android.gms.maps.GoogleMap.OnMyLocationButtonClickListener
import com.google.android.gms.maps.MapView
import com.google.android.gms.maps.OnMapReadyCallback
import com.google.android.gms.maps.model.*
import com.google.android.material.bottomsheet.BottomSheetBehavior
import com.google.android.material.bottomsheet.BottomSheetBehavior.BottomSheetCallback
import kotlinx.android.synthetic.main.bottom_sheet.*
import kotlinx.android.synthetic.main.fragment_map_fragment.*
import org.koin.androidx.viewmodel.ext.android.viewModel
import java.util.*

class MapFragment : Fragment(), OnMapReadyCallback, OnMyLocationButtonClickListener,
    GoogleMap.OnMyLocationClickListener,
    GoogleMap.OnPolylineClickListener, GoogleMap.OnInfoWindowClickListener,
    GoogleMap.OnMapLongClickListener,
    GoogleMap.OnMapClickListener {
    private lateinit var mMap: GoogleMap
    private val mMapView: MapView by lazy { map }
    private val clearMapButton by lazy { fragment_map_clear_markers }

    private val buttonSelectedLines by lazy { fragment_map_selectec_lines_markers }
    private val baseRouteButton500 by lazy { accion_bus_500 }
    private val baseRouteButton501 by lazy { accion_bus_501 }
    private val baseRouteButton502 by lazy { accion_bus_502 }
    private val baseRouteButton503 by lazy { accion_bus_503 }
    private val baseRouteButton504 by lazy { accion_bus_504 }
    private val baseRouteButton505 by lazy { accion_bus_505 }
    private val layoutBottomSheet by lazy { bottom_sheet }
    private val containerDropSheetImage by lazy { bottom_sheet_drop_image }

    private val btmSheetImageOrigin by lazy { label_origin_imageView }
    private val btmSheetImageDestino by lazy { label_destino_image }
    private val btmSheetImageDelete by lazy { label_delete_imageView }

    private val btmSheetTextOrigin by lazy { label_origin_value }
    private val btmSheetTextDestino by lazy { label_destino_value }
    private val btmSheetProceedSearch by lazy { btn_buscar }

    private val imagePuntoOrigen by lazy { imageViewPuntoOrigen }

    private var manualFlag = false
    private var manualPoint = ""

    private lateinit var adapter: CustomInfoWindowAdapter
    private lateinit var sheetBehavior: BottomSheetBehavior<View>

    val mapFragmentViewModel by viewModel<MapFragmentViewModel>()

    companion object {
        const val LOCATION_REQUEST_CODE = 1
        fun newInstance() = MapFragment()
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.fragment_map_fragment, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)


        mapFragmentViewModel.liveData.observe(::getLifecycle, ::updateUI)
        mapFragmentViewModel.setLoading()

        mMapView.onCreate(null)
        mMapView.onResume()
        mMapView.getMapAsync(this)

        setButtonsListeners()

    }

    private fun updateUI(data: Event<MapFragmentViewModel.Data>) {
        val pokemonCardDetailData = data.getContentIfNotHandled()
        when (pokemonCardDetailData?.status) {
            MapFragmentViewModel.Status.LOADING -> {
                setBusLines(data.peekContent().data as MutableList<LineBus>)
            }

            MapFragmentViewModel.Status.SHOW_ROUTES -> setVisibilityMenuButton(
                data.peekContent().data,
                data.peekContent().dataAlternativa
            )
            MapFragmentViewModel.Status.MANUAL_POINT -> setManualPoint(data.peekContent().data)
            MapFragmentViewModel.Status.PROCEED_SEARCHING -> searchOperation(
                data.peekContent().data
            )

            MapFragmentViewModel.Status.ACTIVATE_BUTTON -> btmSheetProceedSearch.isEnabled = true
            MapFragmentViewModel.Status.DEACTIVATE_BUTTON -> {
                btmSheetTextOrigin.text = ""
                btmSheetTextDestino.text = ""
                btmSheetProceedSearch.isEnabled = false
            }
            MapFragmentViewModel.Status.AWAY_MARKERS -> {

                mapFragmentViewModel.showAutoLocation()
            }
            MapFragmentViewModel.Status.SHOW_LOC -> {
                if (mapFragmentViewModel.activeLine.isNotEmpty()) { // control para no general null pointer si se eliminan las lineas seleccionadas y haya entrado previamente al estado
                    val marcadorA = data.peekContent().data as Coordinates
                    val marcadorB = data.peekContent().dataAlternativa as Coordinates

                    mMap.clear()
                    setRoutes(
                        mapFragmentViewModel.listRecorridoIda,
                        mapFragmentViewModel.listRecorridoVuelta
                    )
                    val mMarkerTest: Marker = mMap.addMarker(
                        MarkerOptions()
                            .position(LatLng(marcadorA.latitude, marcadorA.longitude))
                            .icon(
                                ViewUtils.bitmapDescriptorFromVector(
                                    requireContext(),
                                    getBusIcon(mapFragmentViewModel.activeLine[0].toString())
                                )
                            )
                    )
                    val mMarkerBTest: Marker = mMap.addMarker(
                        MarkerOptions()
                            .position(LatLng(marcadorB.latitude, marcadorB.longitude))
                            .icon(
                                ViewUtils.bitmapDescriptorFromVector(
                                    requireContext(),
                                    getBusIcon(mapFragmentViewModel.activeLine[0].toString())
                                )
                            )
                    )
                    mapFragmentViewModel.addSimBusMarker(mMarkerTest, mMarkerBTest)
                    mapFragmentViewModel.showAutoLocation()
                }
            }
            MapFragmentViewModel.Status.ERROR -> {

                invokeAlertDialog(
                    activity = requireActivity(),
                    message = data.peekContent().data.toString(),
                    positiveButtonS = data.peekContent().dataAlternativa.toString()
                )
            }
            MapFragmentViewModel.Status.ENABLE_GOTO_BUTTON -> buttonSelectedLines.visibility =
                View.VISIBLE
            MapFragmentViewModel.Status.DISABLE_GOTO_BUTTON -> buttonSelectedLines.visibility =
                View.GONE
            MapFragmentViewModel.Status.CLEAN_MAP -> cleanMapAndMarkers()
        }
    }

    private fun setButtonsListeners() {

        clearMapButton.onClickThrottled({
            cleanMapAndMarkers()
        })
        containerDropSheetImage.setOnClickListener {
            onClickOriginDestinoButton()
            toggleBottomSheet()
        }
        btmSheetProceedSearch.setOnClickListener {
            mapFragmentViewModel.proceedSearching()
        }
        imagePuntoOrigen.onClickThrottled({
            goToMyLocation()
            val address = MapUtils.getAddressByLatLng(
                requireContext(),
                mapFragmentViewModel.myLocation
            )
            manualPoint = "ORIGIN"
            btmSheetTextOrigin.text = address.name

            mapFragmentViewModel.setManualOriginPoint(address)
        }, 5000L)
        buttonSelectedLines.visibility = View.GONE
        buttonSelectedLines.onClickThrottled({
            goToMyLocation()
            manualPoint = "ORIGIN"
            val address = MapUtils.getAddressByLatLng(
                requireContext(),
                mapFragmentViewModel.myLocation
            )
            btmSheetTextOrigin.text = address.name

            mapFragmentViewModel.setManualOriginPoint(address)

            if (btmSheetTextDestino.text.isNotEmpty()) {
                mapFragmentViewModel.proceedSearching()
            }
        })


        sheetBehavior = BottomSheetBehavior.from(layoutBottomSheet)

        /**
         * bottom sheet state change listener
         * we are changing button text when sheet changed state
         * */
        /**
         * bottom sheet state change listener
         * we are changing button text when sheet changed state
         */
        sheetBehavior.setBottomSheetCallback(object : BottomSheetCallback() {
            override fun onStateChanged(
                bottomSheet: View,
                newState: Int
            ) {
                when (newState) {
                    BottomSheetBehavior.STATE_HIDDEN -> {
                    }
                    BottomSheetBehavior.STATE_EXPANDED -> {
                    }
                    BottomSheetBehavior.STATE_COLLAPSED -> {
                    }
                    BottomSheetBehavior.STATE_DRAGGING -> {
                    }
                    BottomSheetBehavior.STATE_SETTLING -> {
                    }
                    BottomSheetBehavior.STATE_HALF_EXPANDED -> {
                    }
                }
            }

            override fun onSlide(
                bottomSheet: View,
                slideOffset: Float
            ) {
            }
        })
    }

    private fun toggleBottomSheet() {
        if (sheetBehavior.state != BottomSheetBehavior.STATE_EXPANDED) {
            sheetBehavior.setState(BottomSheetBehavior.STATE_EXPANDED)
        } else {
            sheetBehavior.setState(BottomSheetBehavior.STATE_COLLAPSED)
        }
    }

    /**
     * OPERACIONES SOBRE LINEAS Y RECORRIDOS
     *
     * - setBusLines: muesrta las lineas disponibles por el servidor
     * - setRoutes: pinta el recorrido y marcadores de cada linea clickeada
     */

    private fun setBusLines(busLines: MutableList<LineBus>) {
        when (busLines.size) {
            1 -> {

                baseRouteButton500.visibility =
                    if (busLines[0].linea.contains("500")) View.VISIBLE else View.GONE
            }
            2 -> {

                baseRouteButton500.visibility =
                    if (busLines[0].linea.contains("500")) View.VISIBLE else View.GONE
                baseRouteButton501.visibility =
                    if (busLines[1].linea.contains("501")) View.VISIBLE else View.GONE
            }
            3 -> {
                baseRouteButton500.visibility =
                    if (busLines[0].linea.contains("500")) View.VISIBLE else View.GONE
                baseRouteButton501.visibility =
                    if (busLines[1].linea.contains("501")) View.VISIBLE else View.GONE
                baseRouteButton502.visibility =
                    if (busLines[2].linea.contains("502")) View.VISIBLE else View.GONE
            }
            4 -> {
                baseRouteButton500.visibility =
                    if (busLines[0].linea.contains("500")) View.VISIBLE else View.GONE
                baseRouteButton501.visibility =
                    if (busLines[1].linea.contains("501")) View.VISIBLE else View.GONE
                baseRouteButton502.visibility =
                    if (busLines[2].linea.contains("502")) View.VISIBLE else View.GONE
                baseRouteButton503.visibility =
                    if (busLines[3].linea.contains("503")) View.VISIBLE else View.GONE
            }
            5 -> {
                baseRouteButton500.visibility =
                    if (busLines[0].linea.contains("500")) View.VISIBLE else View.GONE
                baseRouteButton501.visibility =
                    if (busLines[1].linea.contains("501")) View.VISIBLE else View.GONE
                baseRouteButton502.visibility =
                    if (busLines[2].linea.contains("502")) View.VISIBLE else View.GONE
                baseRouteButton503.visibility =
                    if (busLines[3].linea.contains("503")) View.VISIBLE else View.GONE
                baseRouteButton504.visibility =
                    if (busLines[4].linea.contains("504")) View.VISIBLE else View.GONE
            }
            6 -> {
                baseRouteButton500.visibility =
                    if (busLines[0].linea.contains("500")) View.VISIBLE else View.GONE
                baseRouteButton501.visibility =
                    if (busLines[1].linea.contains("501")) View.VISIBLE else View.GONE
                baseRouteButton502.visibility =
                    if (busLines[2].linea.contains("502")) View.VISIBLE else View.GONE
                baseRouteButton503.visibility =
                    if (busLines[3].linea.contains("503")) View.VISIBLE else View.GONE
                baseRouteButton504.visibility =
                    if (busLines[4].linea.contains("504")) View.VISIBLE else View.GONE
                baseRouteButton505.visibility =
                    if (busLines[5].linea.contains("505")) View.VISIBLE else View.GONE
            }
        }

    }

    private fun setRoutes(
        listLatLong: MutableList<RecorridoBaseInformation>,
        mutableList: MutableList<RecorridoBaseInformation>
    ) {
        if (listLatLong.isNotEmpty() || mutableList.isNotEmpty()) {
            val recIda =
                listLatLong[0].coordenadas
            val recVuelta =
                mutableList[0].coordenadas
            val listLatLng = mutableListOf<LatLng>()
            val listLatLng2 = mutableListOf<LatLng>()
            for (i in recIda) {
                val lat = LatLng(i.latitude, i.longitude)
                listLatLng.add(lat)
            }

            for (i in recVuelta) {
                val lat = LatLng(i.latitude, i.longitude)
                listLatLng2.add(lat)
            }

            mMap.addPolyline(
                PolylineOptions()
                    .clickable(true)
                    .addAll(
                        listLatLng
                    ).color(Color.BLUE)
            )

            mMap.addPolyline(
                PolylineOptions()
                    .clickable(true)
                    .addAll(
                        listLatLng2
                    ).color(Color.RED)
            )
            mMap.setOnPolylineClickListener(this)
        }
    }

    private fun onClickOriginDestinoButton() {
        btmSheetImageOrigin.onClickThrottled({

            manualFlag = true
            manualPoint = "ORIGIN"
            btmSheetImageDelete.visibility = View.VISIBLE
            sheetBehavior.state = BottomSheetBehavior.STATE_COLLAPSED
        })
        btmSheetImageDestino.onClickThrottled({
            manualFlag = true
            manualPoint = "DESTINO"
            btmSheetImageDelete.visibility = View.VISIBLE
            sheetBehavior.state = BottomSheetBehavior.STATE_COLLAPSED
        })

        btmSheetImageDelete.onClickThrottled({
            manualFlag = false
            manualPoint = ""
            btmSheetTextOrigin.text = ""
            btmSheetTextDestino.text = ""
            btmSheetImageDelete.visibility = View.GONE
            cleanMapAndMarkers()
        })
    }

    private fun cleanMapAndMarkers() {

        mMap.clear()
        mapFragmentViewModel.cleanMarkers()
    }

    private fun setManualPoint(valor: Any?) {
        val address = valor as Address?
        if (manualPoint == "ORIGIN") {
            btmSheetTextOrigin.text = address?.name
        } else {

            btmSheetTextDestino.text = address?.name
        }

        sheetBehavior.state = BottomSheetBehavior.STATE_EXPANDED
    }

    /**
     * OPERACIONES SOBRE EL MAPA
     *
     * - Permisos y acciones propias del Mapa que necesitan de GoogleMap.
     * - InfoWindowAdapter
     */

    override fun onMapReady(googleMap: GoogleMap) {
        mMap = googleMap
        mMap.mapType = GoogleMap.MAP_TYPE_NORMAL
        if (ContextCompat.checkSelfPermission(
                requireContext(),
                Manifest.permission.ACCESS_FINE_LOCATION
            )
            == PackageManager.PERMISSION_GRANTED
        ) {
            mMap.isMyLocationEnabled = true
            mMap.uiSettings.isMyLocationButtonEnabled = true
            goToMyLocation()
        } else {
            if (ActivityCompat.shouldShowRequestPermissionRationale(
                    requireActivity(),
                    Manifest.permission.ACCESS_FINE_LOCATION
                )
            ) {
                // Mostrar diálogo explicativo
            } else {
                // Solicitar permiso
                ActivityCompat.requestPermissions(
                    requireActivity(),
                    arrayOf(Manifest.permission.ACCESS_FINE_LOCATION),
                    LOCATION_REQUEST_CODE
                )
            }
        }

        mMap.uiSettings.isZoomControlsEnabled = true
        googleMap.moveCamera(CameraUpdateFactory.newLatLngZoom(LatLng(-37.330472, -59.112383), 13f))

        baseRouteButton500.setOnLongClickListener {

            if (!mapFragmentViewModel.activeLineButtonFlag1) {

                mapFragmentViewModel.activeLine.add(500)
                baseRouteButton500.size = FloatingActionButton.SIZE_NORMAL

                mapFragmentViewModel.activeLineButtonFlag1 = true
                mapFragmentViewModel.setGoToButton()
                return@setOnLongClickListener true
            } else {

                mapFragmentViewModel.activeLine.remove(500)
                baseRouteButton500.size = FloatingActionButton.SIZE_MINI

                mapFragmentViewModel.activeLineButtonFlag1 = false
                mapFragmentViewModel.setGoToButton()
                return@setOnLongClickListener true
            }
        }
        baseRouteButton501.setOnLongClickListener {

            if (!mapFragmentViewModel.activeLineButtonFlag2) {

                mapFragmentViewModel.activeLine.add(501)
                baseRouteButton501.size = FloatingActionButton.SIZE_NORMAL

                mapFragmentViewModel.activeLineButtonFlag2 = true
                mapFragmentViewModel.setGoToButton()
                return@setOnLongClickListener true
            } else {

                mapFragmentViewModel.activeLine.remove(501)
                baseRouteButton501.size = FloatingActionButton.SIZE_MINI
                mapFragmentViewModel.activeLineButtonFlag2 = false
                mapFragmentViewModel.setGoToButton()
                return@setOnLongClickListener true
            }
        }
        baseRouteButton502.setOnLongClickListener {
            if (!mapFragmentViewModel.activeLineButtonFlag3) {
                mapFragmentViewModel.activeLine.add(502)

                baseRouteButton502.size = FloatingActionButton.SIZE_NORMAL
                mapFragmentViewModel.activeLineButtonFlag3 = true
                mapFragmentViewModel.setGoToButton()
                return@setOnLongClickListener true
            } else {
                mapFragmentViewModel.activeLineButtonFlag3 = false
                baseRouteButton502.size = FloatingActionButton.SIZE_MINI

                mapFragmentViewModel.activeLine.remove(502)
                mapFragmentViewModel.setGoToButton()
                return@setOnLongClickListener true
            }
        }
        baseRouteButton503.setOnLongClickListener {
            if (!mapFragmentViewModel.activeLineButtonFlag4) {
                buttonSelectedLines.visibility = View.VISIBLE

                mapFragmentViewModel.activeLine.add(503)
                baseRouteButton503.size = FloatingActionButton.SIZE_NORMAL

                mapFragmentViewModel.activeLineButtonFlag4 = true
                mapFragmentViewModel.setGoToButton()
                return@setOnLongClickListener true
            } else {

                mapFragmentViewModel.activeLine.remove(503)
                baseRouteButton503.size = FloatingActionButton.SIZE_MINI
                mapFragmentViewModel.activeLineButtonFlag4 = false
                mapFragmentViewModel.setGoToButton()
                return@setOnLongClickListener true
            }
        }
        baseRouteButton504.setOnLongClickListener {
            if (!mapFragmentViewModel.activeLineButtonFlag5) {
                buttonSelectedLines.visibility = View.VISIBLE

                mapFragmentViewModel.activeLine.add(504)
                baseRouteButton504.size = FloatingActionButton.SIZE_NORMAL

                mapFragmentViewModel.activeLineButtonFlag5 = true
                mapFragmentViewModel.setGoToButton()
                return@setOnLongClickListener true
            } else {

                mapFragmentViewModel.activeLine.remove(504)
                baseRouteButton504.size = FloatingActionButton.SIZE_MINI
                mapFragmentViewModel.activeLineButtonFlag5 = false
                mapFragmentViewModel.setGoToButton()
                return@setOnLongClickListener true
            }
        }
        baseRouteButton505.setOnLongClickListener {
            if (!mapFragmentViewModel.activeLineButtonFlag6) {
                buttonSelectedLines.visibility = View.VISIBLE

                mapFragmentViewModel.activeLine.add(505)
                baseRouteButton505.size = FloatingActionButton.SIZE_NORMAL

                mapFragmentViewModel.activeLineButtonFlag6 = true
                mapFragmentViewModel.setGoToButton()
                return@setOnLongClickListener true
            } else {

                mapFragmentViewModel.activeLine.remove(505)
                baseRouteButton505.size = FloatingActionButton.SIZE_MINI
                mapFragmentViewModel.activeLineButtonFlag6 = false
                mapFragmentViewModel.setGoToButton()
                return@setOnLongClickListener true
            }
        }

        baseRouteButton500.onClickThrottled({

            mapFragmentViewModel.showBaseRoute(500)
        })
        baseRouteButton501.onClickThrottled({

            mapFragmentViewModel.showBaseRoute(501)
        })
        baseRouteButton502.onClickThrottled({

            mapFragmentViewModel.showBaseRoute(502)
        })
        baseRouteButton503.onClickThrottled({

            mapFragmentViewModel.showBaseRoute(503)
        })
        baseRouteButton504.onClickThrottled({

            mapFragmentViewModel.showBaseRoute(504)
        })
        baseRouteButton505.onClickThrottled({

            mapFragmentViewModel.showBaseRoute(505)
        })

        mMap.setOnMapClickListener(this)

        mMap.setOnMapLongClickListener(this)

        adapter = CustomInfoWindowAdapter(
            LayoutInflater.from(activity),
            requireContext(),
            mapFragmentViewModel
        )

        mMap.setInfoWindowAdapter(adapter)
        googleMap.setOnInfoWindowClickListener { marker ->

            mapFragmentViewModel.checkLocation = false
            val address = MapUtils.getAddress(requireContext(), marker)
            manualPoint = "DESTINO"
            mapFragmentViewModel.addressDestination = address
            btmSheetTextDestino.text = address.name
            toggleBottomSheet()
            mapFragmentViewModel.setManualDestPoint(address)
        }
        googleMap.setOnInfoWindowCloseListener {
            mapFragmentViewModel.removeActiveLine()
            mapFragmentViewModel.userAwaitBusRoutes = false
            mapFragmentViewModel.checkLocation = true
            mapFragmentViewModel.showAutoLocation()
        }
    }

    /**
     * OPERACIONES PARA BUSCAR UN LUGAR
     *
     * - searchOperation: limpia los campos y colapsa el modal de informacion de puntos selecionados
     *
     * - showIntermediateTravel: pinta el recorrido intermedio y marcadores de las paradas
     *              de las lineas seleccionadas.
     *
     * - goToFragTravelPrediction: crea el fragmento FragmentTravelPrediction para mostrar
     *              la ventana informativa con los resultados de cada viaje
     */

    private fun searchOperation(data: Any?) {

        btmSheetTextOrigin.text = ""
        btmSheetTextDestino.text = ""
        btmSheetImageDelete.visibility = View.GONE
        sheetBehavior.state = BottomSheetBehavior.STATE_COLLAPSED
        showIntermediateTravel(data)
    }

    private fun showIntermediateTravel(data: Any?) {
        val travel = data as List<TravelLineInformation>

        val listLatLng = mutableListOf<LatLng>()

        val listOfParadas = mutableListOf<InfoPuntoParada>()
        travel.forEach {

            for (i in it.coordenadasIntermedias) {
                val lat = LatLng(i.latitude, i.longitude)
                listLatLng.add(lat)
            }
            mMap.addPolyline(
                PolylineOptions()
                    .clickable(true)
                    .addAll(
                        listLatLng
                    ).color(ContextCompat.getColor(requireContext(), getBusColorRoute(it.linea)))
            )

            val markerStopBusOrigin = mMap.addMarker(
                MarkerOptions()
                    .position(
                        LatLng(
                            it.coordenadasIntermedias[0].latitude,
                            it.coordenadasIntermedias[0].longitude
                        )
                    )
                    .icon(
                        ViewUtils.bitmapDescriptorFromVector(
                            requireContext(),
                            R.drawable.ic_parada_de_autobus
                        )
                    )
            )
            val markerStopBusDestiny = mMap.addMarker(
                MarkerOptions()
                    .position(
                        LatLng(
                            it.coordenadasIntermedias[it.coordenadasIntermedias.size - 1].latitude,
                            it.coordenadasIntermedias[it.coordenadasIntermedias.size - 1].longitude
                        )
                    )
                    .icon(
                        ViewUtils.bitmapDescriptorFromVector(
                            requireContext(),
                            R.drawable.ic_parada_de_autobus
                        )
                    )
            )
            listLatLng.clear()

            listOfParadas.add(
                InfoPuntoParada(
                    it.coordenadasIntermedias[0],
                    MapUtils.getAddress(requireContext(), markerStopBusOrigin),
                    it.coordenadasIntermedias[it.coordenadasIntermedias.size - 1],
                    MapUtils.getAddress(requireContext(), markerStopBusDestiny),
                    "",
                    it.trayecto.toInt(),
                    it.linea.toInt(),
                    1
                )
            )
        }
        goToFragTravelPrediction(listOfParadas)
    }

    private fun goToFragTravelPrediction(listOfParadas: MutableList<InfoPuntoParada>) {

        val ft: FragmentTransaction =
            (context as MainActivity).supportFragmentManager
                .beginTransaction()

        ft.setCustomAnimations(
            R.anim.slide_in,
            R.anim.face_out,
            R.anim.face_in,
            R.anim.slide_out
        )
        ft.replace(
            R.id.account,
            FragmentTravelPrediction.newInstance(
                listOfParadas as ArrayList<InfoPuntoParada>,
                mapFragmentViewModel.activeAlgorithm
            )
        )
        ft.addToBackStack(null)
        ft.commit()
    }

    override fun onMyLocationButtonClick(): Boolean {
        return true
    }

    override fun onPolylineClick(p0: Polyline?) {
    }

    private fun setVisibilityMenuButton(listLatLong: Any?, dataAlternativa: Any?) {

        if (!mapFragmentViewModel.visibleOptions) {
            mapFragmentViewModel.visibleOptions = true
            setRoutes(
                listLatLong as MutableList<RecorridoBaseInformation>,
                dataAlternativa as MutableList<RecorridoBaseInformation>
            )

        } else {
            mapFragmentViewModel.visibleOptions = false
            mMap.clear()
        }
    }

    override fun onMapClick(puntoPulsado: LatLng) {}

    override fun onInfoWindowClick(p0: Marker) {}

    override fun onMapLongClick(p0: LatLng) {

        val mMarkerTest: Marker = mMap.addMarker(
            MarkerOptions()
                .position(p0)
        )
        mapFragmentViewModel.addMarker(mMarkerTest)

        if (manualFlag) {
            manualFlag = false
            val address =
                MapUtils.getAddress(requireContext(), mMarkerTest)
            if (manualPoint == "ORIGIN") {
                mapFragmentViewModel.setManualOriginPoint(address)
            } else {
                mapFragmentViewModel.setManualDestPoint(address)

            }
        }
    }

    override fun onMyLocationClick(p0: Location) {}

    private fun goToMyLocation() {
        if (ActivityCompat.checkSelfPermission(
                requireContext(),
                Manifest.permission.ACCESS_FINE_LOCATION
            ) != PackageManager.PERMISSION_GRANTED && ActivityCompat.checkSelfPermission(
                requireContext(),
                Manifest.permission.ACCESS_COARSE_LOCATION
            ) != PackageManager.PERMISSION_GRANTED
        ) {
            LocationServices.getFusedLocationProviderClient(requireContext()).lastLocation
                .addOnSuccessListener {
                    mMap.isMyLocationEnabled = true
                    mMap.animateCamera(
                        CameraUpdateFactory.newLatLngZoom(
                            LatLng(
                                it?.latitude ?: 0.0, it?.longitude ?: 0.0
                            ), 12F
                        )
                    )
                }
        } else {
            LocationServices.getFusedLocationProviderClient(requireContext()).lastLocation
                .addOnSuccessListener {

                    mapFragmentViewModel.myLocation =
                        LatLng(it?.latitude ?: 0.0, it?.longitude ?: 0.0)
                    mMap.isMyLocationEnabled = true
                    mMap.animateCamera(
                        CameraUpdateFactory.newLatLngZoom(
                            LatLng(
                                it?.latitude ?: 0.0, it?.longitude ?: 0.0
                            ), 12F
                        )
                    )
                }
        }
    }
}